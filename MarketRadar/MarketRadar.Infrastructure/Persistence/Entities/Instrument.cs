namespace MarketRadar.Infrastructure.Persistence.Entities;

public sealed class Instrument
{
    public long Id { get; set; }
    public string Symbol { get; set; } = null!;
}