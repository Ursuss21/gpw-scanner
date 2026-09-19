public class EV_EBITDA : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required EV EV { get; init; }
    public required EBIT EBIT { get; init; }
    public required Amortization Amortization { get; init; }
}