public class AverageOperatingProfitPerShare : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required OperatingProfitPerShare[] HistoricalValues { get; init; }
}