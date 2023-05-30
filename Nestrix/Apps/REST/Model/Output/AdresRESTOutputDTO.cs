namespace RESTLayer.Model.Output;

public class AdresRESTOutputDTO
{
    public Guid Id { get; set; }
    public string Straat { get; set; }
    public string Huisnummer { get; set; }
    public string Postcode { get; set; }
    public string Gemeente { get; set; }
    public string Land { get; set; }
}