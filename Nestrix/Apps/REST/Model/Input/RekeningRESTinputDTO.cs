using LogicLayer.Model;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace RESTLayer.Model.Input;

public class RekeningRESTinputDTO
{
    public Guid BankId { get; set; }
    public Guid GebruikerId { get; set; }

    [JsonConverter(typeof(StringEnumConverter))]
    public RekeningType RekeningType { get; set; }
    public string Iban { get; set; }

    public decimal KredietLimiet { get; set; }
}