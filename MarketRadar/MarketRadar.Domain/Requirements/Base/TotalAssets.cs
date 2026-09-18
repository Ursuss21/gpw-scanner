public class TotalAssets : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
}