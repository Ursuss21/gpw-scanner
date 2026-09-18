public class PriceChange : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required Price PriceCurrent { get; init; }
    public required Price PricePrevious { get; init; }
}