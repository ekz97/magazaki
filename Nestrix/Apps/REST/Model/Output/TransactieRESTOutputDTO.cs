using LogicLayer.Model;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;

namespace RESTLayer.Model.Output;

public class TransactieRESTOutputDTO
{
    public Guid TransactieId { get; set; }
    public decimal Bedrag { get; set; }
    public DateTime Datum { get; set; }
    public string? Omschrijving { get; set; }
    [JsonConverter(typeof(StringEnumConverter))]
    public TransactieType TransactieType { get; set; }
}