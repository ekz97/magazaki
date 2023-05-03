using Peasie.Contracts;

namespace FinancialInstituteAPI.Interfaces
{
    public interface IPaymentTransactionService
    {
        PaymentTransactionDTO Process(PaymentTransactionDTO transaction);
    }
}