public class NetIncome : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
}