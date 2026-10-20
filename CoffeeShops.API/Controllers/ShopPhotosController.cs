using CoffeeShops.API.Data;
using CoffeeShops.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShops.API.Controllers;

/// <summary>
/// Stable photo URLs for the app. The app only ever sees these URLs; what's behind them
/// (Google Places today, owner-uploaded photos later) can change without an app release.
///
///   GET /shops/{placeId}/photos            list for the detail carousel, with attributions
///   GET /shops/{placeId}/photo?i=0&w=800   302 to the image itself
/// </summary>
[ApiController]
[Route("shops/{placeId}")]
public class ShopPhotosController : ControllerBase
{
    // Snap requested widths so the cache isn't fragmented by every device size.
    private static readonly int[] WidthBuckets = { 400, 800, 1200, 1600 };
    private const int ClientCacheSeconds = 6 * 60 * 60;

    private readonly CoffeeShopsDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly GooglePlacePhotoService _photos;

    public ShopPhotosController(CoffeeShopsDbContext context, ICacheService cacheService, GooglePlacePhotoService photos)
    {
        _context = context;
        _cacheService = cacheService;
        _photos = photos;
    }

    [HttpGet("photos")]
    public async Task<IActionResult> List(string placeId, CancellationToken ct)
    {
        if (!_photos.IsConfigured) return StatusCode(StatusCodes.Status503ServiceUnavailable);
        if (!await ShopExistsAsync(placeId, ct)) return NotFound();

        var photos = await _photos.GetPhotosAsync(placeId, ct);
        if (photos == null) return StatusCode(StatusCodes.Status502BadGateway);

        Response.Headers.CacheControl = $"public, max-age={ClientCacheSeconds}";
        return Ok(new
        {
            photos = photos.Select((p, i) => new
            {
                path = $"/shops/{Uri.EscapeDataString(placeId)}/photo?i={i}",
                width = p.WidthPx,
                height = p.HeightPx,
                // Google requires showing the photo's author where the photo is displayed.
                attributions = (p.AuthorAttributions ?? new()).Select(a => new { name = a.DisplayName, uri = a.Uri }),
            }),
        });
    }

    [HttpGet("photo")]
    public async Task<IActionResult> Photo(string placeId, [FromQuery] int i = 0, [FromQuery] int w = 800, CancellationToken ct = default)
    {
        if (!_photos.IsConfigured) return StatusCode(StatusCodes.Status503ServiceUnavailable);
        if (i < 0 || i >= GooglePlacePhotoService.MaxPhotos) return BadRequest();
        if (!await ShopExistsAsync(placeId, ct)) return NotFound();

        var photos = await _photos.GetPhotosAsync(placeId, ct);
        if (photos == null) return StatusCode(StatusCodes.Status502BadGateway);
        if (i >= photos.Count) return NotFound();

        var uri = await _photos.GetPhotoUriAsync(photos[i].Name, SnapWidth(w), ct);
        if (uri == null) return StatusCode(StatusCodes.Status502BadGateway);

        // Shorter than the server-side URI cache, so a device never holds a dead redirect.
        Response.Headers.CacheControl = $"public, max-age={ClientCacheSeconds / 2}";
        return Redirect(uri);
    }

    // Only serve shops in our own data, so the endpoint can't be used to proxy arbitrary places.
    private async Task<bool> ShopExistsAsync(string placeId, CancellationToken ct)
    {
        var cacheKey = $"shop_exists_{placeId}";
        var cached = _cacheService.Get<string>(cacheKey);
        if (cached != null) return cached == "1";

        var exists = await _context.CoffeeShops.AnyAsync(s => s.PlaceId == placeId && s.IsDeleted != true, ct);
        _cacheService.Set(cacheKey, exists ? "1" : "0", TimeSpan.FromHours(1));
        return exists;
    }

    internal static int SnapWidth(int requested) =>
        WidthBuckets.FirstOrDefault(b => b >= requested, WidthBuckets[^1]);
}
