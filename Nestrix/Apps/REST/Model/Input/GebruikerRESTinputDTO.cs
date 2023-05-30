using System.ComponentModel.DataAnnotations;

namespace RESTLayer.Model.Input;

public class GebruikerRESTinputDTO
{
    public string Familienaam { get; set; }
    public string Voornaam { get; set; }
    public string Email { get; set; }
    public string Telefoonnummer { get; set; }
    public string Code { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Geboortedatum must be in the format dd/mm/yyyy")]
    public string Geboortedatum { get; set; }

    public AdresRESTinputDTO Adres { get; set; }
}