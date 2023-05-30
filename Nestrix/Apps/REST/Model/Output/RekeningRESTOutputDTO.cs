namespace RESTLayer.Model.Output;

public class RekeningRESTOutputDTO
{
    public Guid Rekeningnummer { get; set; }
    public string RekeningType { get; set; }
    public decimal KredietLimiet { get; set; }
    public decimal Saldo { get; set; }
    public List<TransactieRESTOutputDTO> Transacties { get; set; }
    public GebruikerRESTOutputDTO Gebruiker { get; set; }
}