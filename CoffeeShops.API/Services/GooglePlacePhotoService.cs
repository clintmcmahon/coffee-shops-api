using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Caching.Memory;

namespace CoffeeShops.API.Services;

/// <summary>
/// Resolves a shop's current Google photos through the Places API (New).
///
/// Stored photo URLs from the harvester expire within weeks, so the app asks this API
/// instead and gets redirected to a fresh image. Photo names and image URIs are short-lived
/// on Google's side too, so both are cached in memory for hours, not persisted.
/// </summary>
public class GooglePlacePhotoService
{
    public const int MaxPhotos = 10;

    private static readonly TimeSpan PhotoListTtl = TimeSpan.FromHours(12);
    private static readonly TimeSpan PhotoUriTtl = TimeSpan.FromHours(6);
    private static readonly TimeSpan FailureTtl = TimeSpan.FromMinutes(10);
    private const string PlacesBaseUrl = "https://places.googleapis.com/v1";

    private readonly HttpClient _http;
    private readonly IMemoryCache _cache;
    private readonly ILogger<GooglePlacePhotoService> _logger;
    private readonly string? _apiKey;

    public GooglePlacePhotoService(
        HttpClient http,
        IMemoryCache cache,
        IConfiguration configuration,
        ILogger<GooglePlacePhotoService> logger)
    {
        _http = http;
        _cache = cache;
        _logger = logger;
        _apiKey = configuration["GoogleApi:ApiKey"];
    }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(_apiKey);

    /// <summary>
    /// Up to <see cref="MaxPhotos"/> photos for a place, or an empty list when Google has none.
    /// Returns null when Google couldn't be reached, so callers don't mistake an outage for "no photos".
    /// </summary>
    public async Task<IReadOnlyList<PlacePhoto>?> GetPhotosAsync(string placeId, CancellationToken ct)
    {
        var cacheKey = $"gphotos:{placeId}";
        if (_cache.TryGetValue(cacheKey, out IReadOnlyList<PlacePhoto>? cached)) return cached;

        using var request = new HttpRequestMessage(HttpMethod.Get, $"{PlacesBaseUrl}/places/{Uri.EscapeDataString(placeId)}");
        request.Headers.Add("X-Goog-Api-Key", _apiKey);
        request.Headers.Add("X-Goog-FieldMask", "photos");

        try
        {
            using var response = await _http.SendAsync(request, ct);
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                IReadOnlyList<PlacePhoto> none = Array.Empty<PlacePhoto>();
                _cache.Set(cacheKey, none, PhotoListTtl);
                return none;
            }
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Places photos lookup for {PlaceId} failed: {Status}", placeId, (int)response.StatusCode);
                _cache.Set(cacheKey, (IReadOnlyList<PlacePhoto>?)null, FailureTtl);
                return null;
            }

            var body = await response.Content.ReadFromJsonAsync<PlaceResponse>(JsonOptions, ct);
            IReadOnlyList<PlacePhoto> photos = (body?.Photos ?? new List<PlacePhoto>()).Take(MaxPhotos).ToList();
            _cache.Set(cacheKey, photos, PhotoListTtl);
            return photos;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            _logger.LogWarning(ex, "Places photos lookup for {PlaceId} threw", placeId);
            return null;
        }
    }

    /// <summary>A short-lived image URL for one photo at roughly the requested width.</summary>
    public async Task<string?> GetPhotoUriAsync(string photoName, int maxWidthPx, CancellationToken ct)
    {
        var cacheKey = $"gphotouri:{photoName}:{maxWidthPx}";
        if (_cache.TryGetValue(cacheKey, out string? cached)) return cached;

        var url = $"{PlacesBaseUrl}/{photoName}/media?maxWidthPx={maxWidthPx}&skipHttpRedirect=true";
        using var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("X-Goog-Api-Key", _apiKey);

        try
        {
            using var response = await _http.SendAsync(request, ct);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Places photo media for {PhotoName} failed: {Status}", photoName, (int)response.StatusCode);
                return null;
            }

            var body = await response.Content.ReadFromJsonAsync<PhotoMediaResponse>(JsonOptions, ct);
            if (string.IsNullOrEmpty(body?.PhotoUri)) return null;

            _cache.Set(cacheKey, body.PhotoUri, PhotoUriTtl);
            return body.PhotoUri;
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or JsonException)
        {
            _logger.LogWarning(ex, "Places photo media for {PhotoName} threw", photoName);
            return null;
        }
    }

    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    private sealed class PlaceResponse
    {
        public List<PlacePhoto>? Photos { get; set; }
    }

    private sealed class PhotoMediaResponse
    {
        public string? PhotoUri { get; set; }
    }
}

public sealed class PlacePhoto
{
    public string Name { get; set; } = "";
    public int WidthPx { get; set; }
    public int HeightPx { get; set; }
    public List<PhotoAttribution>? AuthorAttributions { get; set; }
}

public sealed class PhotoAttribution
{
    public string? DisplayName { get; set; }
    public string? Uri { get; set; }

    [JsonPropertyName("photoUri")]
    public string? PhotoUri { get; set; }
}
