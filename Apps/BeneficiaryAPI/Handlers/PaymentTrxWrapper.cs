using Peasie.Contracts;

namespace BeneficiaryAPI.Handlers
{
    public class PaymentTrxWrapper
    {
        public PaymentTransactionDTO PaymentTrxRequest { get; set; }
        public PaymentResponseDTO PaymentResponseDTO { get; set; }
        public List<PaymentTransactionDTO> PaymentTrxReplyies { get; set; } = new();
    }
}