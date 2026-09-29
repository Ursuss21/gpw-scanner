namespace MarketRadar.Infrastructure.Persistence.Entities;

public sealed class MarketData
{
    public long Id { get; set; }
    public long InstrumentId { get; set; }
    public DateTime AvailableAt { get; set; }
    public decimal Price { get; set; }
    public decimal SessionTurnover { get; set; }
    public Instrument Instrument { get; set; } = null!;
}