using System.Text.Json;
using CoffeeShops.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShops.API.Controllers;

/// <summary>
/// Passport offers from partner shops (a free or two-for-one drink, like the paper passports
/// people already buy). Kept in Data/passport-offers.json until there are enough partners to
/// need a table; editing it ships with the next deploy.
///
///   GET /passport/offers   offers active today
/// </summary>
[ApiController]
[Route("passport/offers")]
public class PassportOffersController : ControllerBase
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private readonly ICacheService _cache;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<PassportOffersController> _logger;

    public PassportOffersController(ICacheService cache, IWebHostEnvironment env, ILogger<PassportOffersController> logger)
    {
        _cache = cache;
        _env = env;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow.AddHours(-6)); // Central time is close enough for date ranges
        var all = _cache.Get<List<PassportOffer>>("passport_offers") ?? Load();
        var active = all.Where(o => o.StartsOn <= today && today <= o.EndsOn).ToList();

        Response.Headers.CacheControl = "public, max-age=600";
        return Ok(new { offers = active });
    }

    private List<PassportOffer> Load()
    {
        var path = Path.Combine(_env.ContentRootPath, "Data", "passport-offers.json");
        var offers = new List<PassportOffer>();
        try
        {
            if (System.IO.File.Exists(path))
            {
                var file = JsonSerializer.Deserialize<OffersFile>(System.IO.File.ReadAllText(path), JsonOptions);
                offers = (file?.Offers ?? new()).Where(o => !string.IsNullOrWhiteSpace(o.PlaceId) && !string.IsNullOrWhiteSpace(o.Id)).ToList();
            }
        }
        catch (JsonException ex)
        {
            // A bad edit to the file shouldn't take the endpoint down; serve nothing and log.
            _logger.LogError(ex, "passport-offers.json is not valid JSON");
        }
        _cache.Set("passport_offers", offers, CacheDuration);
        return offers;
    }

    private sealed class OffersFile
    {
        public List<PassportOffer>? Offers { get; set; }
    }
}

public sealed class PassportOffer
{
    public string Id { get; set; } = "";
    public string PlaceId { get; set; } = "";
    public string Title { get; set; } = "";
    public string? Terms { get; set; }
    public DateOnly StartsOn { get; set; }
    public DateOnly EndsOn { get; set; }
}
