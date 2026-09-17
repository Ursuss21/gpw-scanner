

public class Price : IRequirement
{
    public required string Instrument { get; set; }
    public required DateTime ObservationDate { get; set; }
}