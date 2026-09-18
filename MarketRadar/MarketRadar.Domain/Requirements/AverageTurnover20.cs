public class AverageTurnover20 : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required SessionTurnover[] Last20Sessions { get; init; }
}