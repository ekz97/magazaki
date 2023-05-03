using FinancialInstituteAPI.Interfaces;
using Peasie.Contracts;

namespace FinancialInstituteAPI.Services
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        public PaymentTransactionDTO Process(PaymentTransactionDTO transaction)
        {
            transaction.Status = "COMPLETED";
            return transaction;
        }
    }
}