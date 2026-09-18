public class RelativeMomentum : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required Price PriceCurrent { get; init; }
    public required Price PricePrevious { get; init; }
    public required Price BenchmarkPriceCurrent { get; init; }
    public required Price BenchmarkPricePrevious { get; init; }
}