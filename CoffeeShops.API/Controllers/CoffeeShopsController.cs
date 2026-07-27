using CoffeeShops.API.Data;
using CoffeeShops.API.Models;
using CoffeeShops.API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace CoffeeShops.API.Controllers;

/// <summary>
/// OData-enabled endpoint for querying coffee shops.
///
/// Base URL: /odata/CoffeeShops
///
/// Example queries:
///   GET /odata/CoffeeShops
///   GET /odata/CoffeeShops?$filter=City eq 'Minneapolis'
///   GET /odata/CoffeeShops?$filter=State eq 'MN'&$orderby=Name&$top=25&$skip=0
///   GET /odata/CoffeeShops?$filter=Rating ge 4.5&$orderby=Rating desc
///   GET /odata/CoffeeShops?$select=Name,City,State,Rating
///   GET /odata/CoffeeShops?$count=true&$top=10
///   GET /odata/CoffeeShops('some-place-id')
/// </summary>
public class CoffeeShopsController : ODataController
{
    private readonly CoffeeShopsDbContext _context;
    private readonly ICacheService _cacheService;
    private readonly ILogger<CoffeeShopsController> _logger;
    private static readonly TimeSpan CacheDuration = TimeSpan.FromHours(24);

    public CoffeeShopsController(
        CoffeeShopsDbContext context,
        ICacheService cacheService,
        ILogger<CoffeeShopsController> logger)
    {
        _context = context;
        _cacheService = cacheService;
        _logger = logger;
    }

    /// <summary>
    /// Get all non-deleted coffee shops. Supports full OData query syntax via
    /// $filter, $orderby, $top, $skip, $select, $count, and $expand.
    /// </summary>
    [EnableQuery(MaxTop = 0)]
    public IQueryable<CoffeeShop> Get()
    {
        _logger.LogInformation("OData query received for CoffeeShops collection");
        return _context.CoffeeShops.Where(x => x.IsDeleted != true);
    }

    /// <summary>
    /// Get a single coffee shop by its placeId key.
    /// Example: GET /odata/CoffeeShops('ChIJ_some_place_id')
    /// </summary>
    [EnableQuery]
    public async Task<IActionResult> Get([FromRoute] string key)
    {
        var cacheKey = $"shop_{key}";

        var cachedResult = _cacheService.Get<CoffeeShop>(cacheKey);
        if (cachedResult != null)
        {
            return Ok(cachedResult);
        }

        var coffeeShop = await _context.CoffeeShops.FindAsync(key);

        if (coffeeShop == null)
        {
            return NotFound(new { message = $"Coffee shop with placeId '{key}' not found" });
        }

        _cacheService.Set(cacheKey, coffeeShop, CacheDuration);

        return Ok(coffeeShop);
    }
}
