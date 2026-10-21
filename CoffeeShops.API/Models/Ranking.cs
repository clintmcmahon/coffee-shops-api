using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeeShops.API.Models;

/// <summary>One shop on one app install's ranked list. See db/2026-10-21-community-rankings.</summary>
[Table("Rankings")]
public class Ranking
{
    [Key]
    public long RankingId { get; set; }

    [MaxLength(36)]
    public string DeviceId { get; set; } = string.Empty;

    [MaxLength(255)]
    public string PlaceId { get; set; } = string.Empty;

    /// <summary>0 didn't like it, 1 it was fine, 2 liked it.</summary>
    public byte Bucket { get; set; }

    public int Position { get; set; }

    [Column(TypeName = "decimal(3,1)")]
    public decimal Score { get; set; }

    public DateTime UpdatedAt { get; set; }
}
