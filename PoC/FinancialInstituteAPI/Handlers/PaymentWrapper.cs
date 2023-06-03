using Peasie.Contracts;

namespace FinancialInstituteAPI.Handlers
{
    internal class PaymentWrapper
    {
        public PaymentRequestDTO? Request { get; set; }
        public PaymentResponseDTO? Response { get; set; }
        public string? FinancialInstitutePrivateKey { get; set; }
    }
}