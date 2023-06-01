namespace RESTLayer.Model.Output;

public class GebruikerRESTOutputDTO
{
    public Guid Id { get; set; }
    public string? Familienaam { get; set; }
    public string? Voornaam { get; set; }
    public string? Email { get; set; }
    public string? Telefoonnummer { get; set; }
    public DateTime Geboortedatum { get; set; }
    public AdresRESTOutputDTO? Adres { get; set; }
}