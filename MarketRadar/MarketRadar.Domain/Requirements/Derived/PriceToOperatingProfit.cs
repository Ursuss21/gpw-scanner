public class PriceToOperatingProfit : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required MarketCapitalization MarketCapitalization { get; init; }
    public required EBIT EBIT { get; init; }
}