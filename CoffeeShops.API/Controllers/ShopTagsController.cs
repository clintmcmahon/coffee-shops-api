using System.Text.Json;
using CoffeeShops.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShops.API.Controllers;

/// <summary>
/// Hand-curated shop tags that no data source provides, like which shops are in the skyway.
/// Kept in Data/shop-tags.json; editing it ships with the next deploy.
///
///   GET /shops/tags   { "skyway": ["placeId", ...] }
/// </summary>
[ApiController]
[Route("shops/tags")]
public class ShopTagsController : ControllerBase
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(30);

    private readonly ICacheService _cache;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<ShopTagsController> _logger;

    public ShopTagsController(ICacheService cache, IWebHostEnvironment env, ILogger<ShopTagsController> logger)
    {
        _cache = cache;
        _env = env;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        var tags = _cache.Get<Dictionary<string, List<string>>>("shop_tags") ?? Load();
        Response.Headers.CacheControl = "public, max-age=1800";
        return Ok(tags);
    }

    private Dictionary<string, List<string>> Load()
    {
        var tags = new Dictionary<string, List<string>>();
        var path = Path.Combine(_env.ContentRootPath, "Data", "shop-tags.json");
        try
        {
            if (System.IO.File.Exists(path))
            {
                using var doc = JsonDocument.Parse(System.IO.File.ReadAllText(path));
                foreach (var prop in doc.RootElement.EnumerateObject())
                {
                    // Keys starting with "_" are notes and unreviewed candidates, never served.
                    if (prop.Name.StartsWith('_') || prop.Value.ValueKind != JsonValueKind.Array) continue;
                    tags[prop.Name] = prop.Value.EnumerateArray()
                        .Where(v => v.ValueKind == JsonValueKind.String)
                        .Select(v => v.GetString()!)
                        .ToList();
                }
            }
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "shop-tags.json is not valid JSON");
        }
        _cache.Set("shop_tags", tags, CacheDuration);
        return tags;
    }
}
