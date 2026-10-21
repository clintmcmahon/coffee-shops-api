using System.Text.Json;
using CoffeeShops.API.Controllers;
using CoffeeShops.API.Data;
using CoffeeShops.API.Models;
using CoffeeShops.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace CoffeeShops.API.Tests;

public class CommunityScoresTests
{
    [Fact]
    public void Smoothing_pulls_small_samples_toward_the_overall_average()
    {
        // 10 rankings averaging 9.0 against an overall 7.0, k = 5: (90 + 35) / 15
        Assert.Equal(8.333, CommunityScores.Smoothed(10, 9.0, 7.0, 5), 3);
        Assert.Equal(7.0, CommunityScores.Smoothed(0, 0, 7.0, 5), 3);
    }

    [Fact]
    public void Shops_below_the_minimum_are_hidden_and_the_rest_sorted()
    {
        var scores = CommunityScores.Compute(
            new[] { ("a", 12, 8.0), ("b", 9, 10.0), ("c", 30, 9.0) }, globalAverage: 7.5, minRankings: 10, k: 5);
        Assert.Equal(new[] { "c", "a" }, scores.Select(s => s.PlaceId));
    }
}

public class CommunityControllerTests : IDisposable
{
    private readonly SqliteConnection _connection;
    private readonly CoffeeShopsDbContext _db;

    public CommunityControllerTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();
        _db = new CoffeeShopsDbContext(new DbContextOptionsBuilder<CoffeeShopsDbContext>().UseSqlite(_connection).Options);
        _db.Database.EnsureCreated();
        _db.CoffeeShops.AddRange(
            new CoffeeShop { PlaceId = "a", Name = "Alpha", City = "Minneapolis" },
            new CoffeeShop { PlaceId = "b", Name = "Bravo", City = "St Paul" },
            new CoffeeShop { PlaceId = "gone", Name = "Closed", City = "Duluth", IsDeleted = true });
        _db.SaveChanges();
    }

    public void Dispose()
    {
        _db.Dispose();
        _connection.Dispose();
    }

    private CommunityController Controller(int minRankings = 2)
    {
        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?> { ["Community:MinRankings"] = minRankings.ToString() })
            .Build();
        var controller = new CommunityController(_db, new CacheService(new MemoryCache(new MemoryCacheOptions())), config);
        controller.ControllerContext = new ControllerContext { HttpContext = new DefaultHttpContext() };
        return controller;
    }

    private static CommunityController.RankingsBody Body(params (string PlaceId, int Bucket, double Score)[] rows) => new()
    {
        Rankings = rows.Select((r, i) => new CommunityController.RankingInput { PlaceId = r.PlaceId, Bucket = r.Bucket, Position = i, Score = r.Score }).ToList(),
    };

    private static JsonElement Json(IActionResult result) =>
        JsonSerializer.SerializeToElement(Assert.IsType<OkObjectResult>(result).Value);

    [Fact]
    public async Task Replacing_a_list_keeps_only_known_open_shops()
    {
        var device = Guid.NewGuid().ToString();
        var result = Json(await Controller().Replace(device, Body(("a", 2, 9.0), ("gone", 2, 8.0), ("nope", 1, 5.0)), default));
        Assert.Equal(1, result.GetProperty("saved").GetInt32());
        Assert.Equal(2, result.GetProperty("ignored").GetInt32());

        await Controller().Replace(device, Body(("b", 1, 5.5)), default);
        Assert.Equal(new[] { "b" }, _db.Rankings.Where(r => r.DeviceId == device).Select(r => r.PlaceId).ToArray());
    }

    [Fact]
    public async Task An_empty_list_removes_a_device()
    {
        var device = Guid.NewGuid().ToString();
        await Controller().Replace(device, Body(("a", 2, 9.0)), default);
        await Controller().Replace(device, Body(), default);
        Assert.Empty(_db.Rankings.Where(r => r.DeviceId == device));
    }

    [Fact]
    public async Task Bad_input_is_rejected()
    {
        Assert.IsType<BadRequestObjectResult>(await Controller().Replace("not-a-uuid", Body(("a", 2, 9.0)), default));
        Assert.IsType<BadRequestObjectResult>(await Controller().Replace(Guid.NewGuid().ToString(), Body(("a", 3, 9.0)), default));
        Assert.IsType<BadRequestObjectResult>(await Controller().Replace(Guid.NewGuid().ToString(), Body(("a", 2, 11.0)), default));
    }

    [Fact]
    public async Task Top_lists_shops_that_reach_the_minimum()
    {
        foreach (var score in new[] { 9.5, 8.5 })
            await Controller().Replace(Guid.NewGuid().ToString(), Body(("a", 2, score), ("b", 1, 5.0)), default);
        await Controller().Replace(Guid.NewGuid().ToString(), Body(("b", 1, 6.0)), default);

        var top = Json(await Controller(minRankings: 3).Top(10, default));
        var shops = top.GetProperty("shops").EnumerateArray().ToList();
        Assert.Single(shops);
        Assert.Equal("b", shops[0].GetProperty("PlaceId").GetString());
        Assert.Equal("Bravo", shops[0].GetProperty("Name").GetString());
        Assert.Equal(3, shops[0].GetProperty("Count").GetInt32());
        Assert.Equal(3, top.GetProperty("minRankings").GetInt32());
    }
}
