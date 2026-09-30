using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace MarketRadar.Infrastructure.Persistence;

public sealed class MarketRadarDbContextFactory : IDesignTimeDbContextFactory<MarketRadarDbContext>
{
    public MarketRadarDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MarketRadarDbContext>();

        var connectionString = Environment.GetEnvironmentVariable("MARKET_RADAR_CONNECTION_STRING");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException("MARKET_RADAR_CONNECTION_STRING environment variable is not set.");
        }

        optionsBuilder.UseNpgsql(connectionString);

        return new MarketRadarDbContext(optionsBuilder.Options);
    }
}