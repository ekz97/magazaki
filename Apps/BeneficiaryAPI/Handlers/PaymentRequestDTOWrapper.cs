using Peasie.Contracts;

namespace BeneficiaryAPI.Handlers
{
    public class PaymentRequestDTOWrapper
    {
        public PaymentRequestDTO? PaymentRequest { get; set; }
        public PaymentResponseDTO? PaymentResponse { get; set; }
        public string? PrivateKey { get; set; }
        public string? PublicKey { get; set; }
    }
}