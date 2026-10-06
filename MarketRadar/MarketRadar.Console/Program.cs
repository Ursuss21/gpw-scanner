using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MarketRadar.Infrastructure.Persistence;

var builder = Host.CreateApplicationBuilder(args);
var connectionString = Environment.GetEnvironmentVariable("MARKET_RADAR_CONNECTION_STRING");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException("MARKET_RADAR_CONNECTION_STRING environment variable is not set.");
}

builder.Services.AddDbContext<MarketRadarDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<
    IRequirementReader<PriceRequest, PriceResponse>,
    PriceReader>();

using var app = builder.Build();
using var scope = app.Services.CreateScope();

var priceReader = scope.ServiceProvider
    .GetRequiredService<IRequirementReader<PriceRequest, PriceResponse>>();

var response = priceReader.Get(new PriceRequest("ASB", new DateTime(2026, 4, 23, 23, 0, 0)));

if (response.Value is decimal price)
{
    Console.WriteLine($"Price: {price}");
}
else
{
    Console.WriteLine("No price found");
}