using Peasie.Contracts;

namespace RESTLayer.Interfaces
{
    public interface IPaymentTransactionService
    {
        PaymentTransactionDTO Process(PaymentTransactionDTO transaction);
    }
}