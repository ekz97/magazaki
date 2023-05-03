namespace FinancialInstituteAPI.Models
{
    public class Country
    {
        public string Name { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string CurrencyCode { get; set; } = string.Empty;
        public int CurrencyNumber { get; set; }
    }
}
