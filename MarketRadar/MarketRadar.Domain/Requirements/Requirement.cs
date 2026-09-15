public class Requirement
{
    public string Name { get; set; }
    public string Instrument { get; set; }
    public DateTime ObservationDate { get; set; }
    public Requirement[] Requirements { get; set; }
}