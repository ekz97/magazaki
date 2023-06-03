using Peasie.Contracts;
using Peasie.Contracts.Interfaces;

namespace BeneficiaryAPI.Handlers
{
    public class PaymentTrxWrapper: IToHtmlTable
    {
        public PaymentTransactionDTO? Request { get; set; }
        public PaymentResponseDTO? Response { get; set; }
        public List<PaymentTransactionDTO> Updates { get; set; } = new();
    }
}