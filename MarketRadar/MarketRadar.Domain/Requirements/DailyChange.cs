public class DailyChange : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required Price PriceToday { get; init; }
    public required Price PriceYesterday { get; init; }
}