public class AverageTurnover : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required SessionTurnover[] LastSessions { get; init; }
}