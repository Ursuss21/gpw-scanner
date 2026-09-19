public class PriceToBook : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required MarketCapitalization MarketCapitalization { get; init; }
    public required Equity Equity { get; init; }
}