public class TotalDebt : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
    public required TotalLiabilities TotalLiabilities { get; init; }
    public required TotalAssets TotalAssets { get; init; }
}