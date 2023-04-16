using Peasie.Contracts;

namespace FinancialInstituteAPI.Handlers
{
    internal class PaymentWrapper
    {
        public PaymentRequestDTO? PaymentRequest { get; set; }
        public PaymentResponseDTO? PaymentResponse { get; set; }
        public string? FinancialInstitutePrivateKey { get; set; }
    }
}