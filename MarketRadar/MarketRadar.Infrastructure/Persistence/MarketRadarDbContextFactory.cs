using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MarketRadar.Infrastructure.Persistence;

public sealed class MarketRadarDbContextFactory : IDesignTimeDbContextFactory<MarketRadarDbContext>
{
    public MarketRadarDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MarketRadarDbContext>();

        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=market_radar;Username=admin;Password=zaq1@WSX");

        return new MarketRadarDbContext(optionsBuilder.Options);
    }
}