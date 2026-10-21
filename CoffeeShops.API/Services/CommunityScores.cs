namespace CoffeeShops.API.Services;

/// <summary>
/// Community score math from the growth plan: each shop's average ranking score, pulled
/// toward the overall average until it has enough rankings, and hidden below a minimum.
///   score = (n * avg + k * globalAvg) / (n + k)
/// </summary>
public static class CommunityScores
{
    public static double Smoothed(int count, double average, double globalAverage, double k) =>
        (count * average + k * globalAverage) / (count + k);

    public static IReadOnlyList<CommunityScore> Compute(
        IEnumerable<(string PlaceId, int Count, double Average)> perShop,
        double globalAverage,
        int minRankings,
        double k) =>
        perShop
            .Where(s => s.Count >= minRankings)
            .Select(s => new CommunityScore(s.PlaceId, Math.Round(Smoothed(s.Count, s.Average, globalAverage, k), 1), s.Count))
            .OrderByDescending(s => s.Score)
            .ThenByDescending(s => s.Count)
            .ToList();
}

public record CommunityScore(string PlaceId, double Score, int Count);
