public class Price : IRequirement
{
    public required string Instrument { get; init; }
    public required DateTime ObservationDate { get; init; }
}

public record PriceRequest(string Instrument, DateTime At);

public record PriceResponse(decimal Value);