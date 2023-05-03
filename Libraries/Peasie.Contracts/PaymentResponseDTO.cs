using Peasie.Contracts.Interfaces;

namespace Peasie.Contracts
{
    public class PaymentResponseDTO : IToHtmlTable
    {
        public string? SourceGuid { get; set; }
        public string? PaymentSID { get; set; }
        public DateTime ReplyTimeUtc { get; set; }
        public TimeSpan ValidityTimeSpan { get; set; }
        public string? FinancialInstitutePublicKey { get; set; }
    }
}