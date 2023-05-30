using LogicLayer.Model;

namespace RESTLayer.Model.Input;

public class BankRESTinputDTO
{
    public string Naam { get; set; }
    public string Telefoonnummer { get; set; }
    public AdresRESTinputDTO Adres { get; set; }
}