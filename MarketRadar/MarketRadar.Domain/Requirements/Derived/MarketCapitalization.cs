public class MarketCapitalization : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required Price Price { get; init; }
    public required SharesOutstanding SharesOutstanding { get; init; }
}