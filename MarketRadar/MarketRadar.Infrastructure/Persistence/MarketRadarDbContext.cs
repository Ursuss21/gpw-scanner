using MarketRadar.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace MarketRadar.Infrastructure.Persistence;

public sealed class MarketRadarDbContext(DbContextOptions<MarketRadarDbContext> options) : DbContext(options)
{
    public DbSet<Instrument> Instruments => Set<Instrument>();
    public DbSet<MarketData> MarketData => Set<MarketData>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(MarketRadarDbContext).Assembly);
    }
}