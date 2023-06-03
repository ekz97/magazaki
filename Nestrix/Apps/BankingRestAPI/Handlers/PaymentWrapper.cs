using Peasie.Contracts;

namespace RESTLayer.Handlers
{
    internal class PaymentWrapper
    {
        public PaymentRequestDTO? Request { get; set; }
        public PaymentResponseDTO? Response { get; set; }
        public string? FinancialInstitutePrivateKey { get; set; }
    }
}