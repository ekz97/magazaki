namespace Peasie.Contracts
{
    public class PaymentResponseDTO
    {
        public Guid SourceGuid { get; set; }
        public Guid PaymentSID { get; set; }
        public DateTime ReplyTimeUtc { get; set; }
        public TimeSpan ValidityTimeSpan { get; set; }
        public string? FinancialInstitutePublicKey { get; set; }
    }
}