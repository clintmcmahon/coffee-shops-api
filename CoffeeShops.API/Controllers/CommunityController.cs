using CoffeeShops.API.Data;
using CoffeeShops.API.Models;
using CoffeeShops.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShops.API.Controllers;

/// <summary>
/// Community rankings. App installs send their ranked lists anonymously; the API combines
/// them into community scores. Needs the Rankings table (db/2026-10-21-community-rankings).
///
///   PUT /rankings/{deviceId}   replace this install's rankings (an empty list removes them)
///   GET /community/scores      { placeId: { score, count } } for shops with enough rankings
///   GET /community/top?limit=  best shops with name and city
/// </summary>
[ApiController]
public class CommunityController : ControllerBase
{
    private const int MaxRankingsPerDevice = 1000;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

    private readonly CoffeeShopsDbContext _context;
    private readonly ICacheService _cache;
    private readonly int _minRankings;
    private readonly double _smoothing;

    public CommunityController(CoffeeShopsDbContext context, ICacheService cache, IConfiguration config)
    {
        _context = context;
        _cache = cache;
        // Growth plan: hide a shop's score until 10 rankings; smoothing weight k of about 5.
        _minRankings = config.GetValue("Community:MinRankings", 10);
        _smoothing = config.GetValue("Community:Smoothing", 5.0);
    }

    public sealed class RankingInput
    {
        public string PlaceId { get; set; } = "";
        public int Bucket { get; set; }
        public int Position { get; set; }
        public double Score { get; set; }
    }

    public sealed class RankingsBody
    {
        public List<RankingInput> Rankings { get; set; } = new();
    }

    [HttpPut("rankings/{deviceId}")]
    public async Task<IActionResult> Replace(string deviceId, [FromBody] RankingsBody body, CancellationToken ct)
    {
        if (!Guid.TryParse(deviceId, out var parsed)) return BadRequest("deviceId must be a UUID");
        var device = parsed.ToString();
        var input = body?.Rankings ?? new();
        if (input.Count > MaxRankingsPerDevice) return BadRequest($"At most {MaxRankingsPerDevice} rankings");
        if (input.Any(r => r.Bucket is < 0 or > 2 || r.Score is < 0 or > 10 || r.Position < 0)) return BadRequest("Invalid ranking");

        // Only shops we list count; duplicates keep the first entry.
        var requested = input.Select(r => r.PlaceId).Distinct().ToList();
        var known = await _context.CoffeeShops
            .Where(s => requested.Contains(s.PlaceId) && s.IsDeleted != true)
            .Select(s => s.PlaceId)
            .ToListAsync(ct);
        var knownSet = known.ToHashSet();
        var now = DateTime.UtcNow;
        var rows = input
            .Where(r => knownSet.Contains(r.PlaceId))
            .GroupBy(r => r.PlaceId)
            .Select(g => g.First())
            .Select(r => new Ranking
            {
                DeviceId = device,
                PlaceId = r.PlaceId,
                Bucket = (byte)r.Bucket,
                Position = r.Position,
                Score = Math.Round((decimal)r.Score, 1),
                UpdatedAt = now,
            })
            .ToList();

        await using var tx = await _context.Database.BeginTransactionAsync(ct);
        await _context.Rankings.Where(r => r.DeviceId == device).ExecuteDeleteAsync(ct);
        _context.Rankings.AddRange(rows);
        await _context.SaveChangesAsync(ct);
        await tx.CommitAsync(ct);

        return Ok(new { saved = rows.Count, ignored = input.Count - rows.Count });
    }

    [HttpGet("community/scores")]
    public async Task<IActionResult> Scores(CancellationToken ct)
    {
        var scores = await ScoresAsync(ct);
        Response.Headers.CacheControl = "public, max-age=600";
        return Ok(scores.ToDictionary(s => s.PlaceId, s => new { score = s.Score, count = s.Count }));
    }

    [HttpGet("community/top")]
    public async Task<IActionResult> Top([FromQuery] int limit = 25, CancellationToken ct = default)
    {
        limit = Math.Clamp(limit, 1, 100);
        var scores = (await ScoresAsync(ct)).Take(limit).ToList();
        var ids = scores.Select(s => s.PlaceId).ToList();
        var shops = await _context.CoffeeShops
            .Where(s => ids.Contains(s.PlaceId) && s.IsDeleted != true)
            .Select(s => new { s.PlaceId, s.Name, s.City })
            .ToDictionaryAsync(s => s.PlaceId, ct);

        Response.Headers.CacheControl = "public, max-age=600";
        return Ok(new
        {
            minRankings = _minRankings,
            shops = scores
                .Where(s => shops.ContainsKey(s.PlaceId))
                .Select((s, i) => new { rank = i + 1, s.PlaceId, shops[s.PlaceId].Name, shops[s.PlaceId].City, s.Score, s.Count }),
        });
    }

    private async Task<IReadOnlyList<CommunityScore>> ScoresAsync(CancellationToken ct)
    {
        var cached = _cache.Get<IReadOnlyList<CommunityScore>>("community_scores");
        if (cached != null) return cached;

        var perShop = await _context.Rankings
            .GroupBy(r => r.PlaceId)
            .Select(g => new { PlaceId = g.Key, Count = g.Count(), Average = g.Average(r => (double)r.Score) })
            .ToListAsync(ct);
        var total = perShop.Sum(s => s.Count);
        var globalAverage = total == 0 ? 0 : perShop.Sum(s => s.Average * s.Count) / total;

        var scores = CommunityScores.Compute(perShop.Select(s => (s.PlaceId, s.Count, s.Average)), globalAverage, _minRankings, _smoothing);
        _cache.Set("community_scores", scores, CacheDuration);
        return scores;
    }
}
