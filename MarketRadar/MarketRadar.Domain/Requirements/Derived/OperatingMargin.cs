public class OperatingMargin : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required EBIT EBIT { get; init; }
    public required SalesRevenue SalesRevenue { get; init; }
}