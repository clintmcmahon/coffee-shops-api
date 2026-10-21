using Microsoft.EntityFrameworkCore;
using CoffeeShops.API.Models;

namespace CoffeeShops.API.Data;

public class CoffeeShopsDbContext : DbContext
{
    public CoffeeShopsDbContext(DbContextOptions<CoffeeShopsDbContext> options) 
        : base(options)
    {
    }

    public DbSet<CoffeeShop> CoffeeShops { get; set; }

    public DbSet<Ranking> Rankings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<CoffeeShop>(entity =>
        {
            entity.HasKey(e => e.PlaceId);
            entity.ToTable("CoffeeShops");
        });
    }
}
