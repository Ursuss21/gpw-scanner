public class ROE : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required NetIncome NetIncome { get; init; }
    public required Equity Equity { get; init; }
}