using Peasie.Contracts;

namespace BeneficiaryAPI.Handlers
{
    public class PaymentRequestDTOWrapper
    {
        public PaymentRequestDTO? Request { get; set; }
        public PaymentResponseDTO? Response { get; set; }
        public string? PrivateKey { get; set; }
        public string? PublicKey { get; set; }
    }
}