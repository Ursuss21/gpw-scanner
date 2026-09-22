public class NetIncome : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
}

public record NetIncomeRequest(string Instrument, DateTime At);

public record NetIncomeResponse(decimal Value);