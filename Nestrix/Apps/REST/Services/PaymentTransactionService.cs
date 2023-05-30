using DataLayer.Repositories;
using LogicLayer.Managers;
using Peasie.Contracts;
using RESTLayer.Interfaces;

namespace RESTLayer.Services
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private RekeningManager _rekeningManager;
        public PaymentTransactionDTO Process(PaymentTransactionDTO transaction)
        {
            string connectionString = "Server=db4free.net;port=3306;user=glenncolombie;password=Nestrix123;database=nestrixdb";
            _rekeningManager = new(new RekeningRepository(connectionString), new TransactieRepository(connectionString));
            var source = Guid.Parse("3c146461-8b97-4028-8a51-3511113d3e95");
            var rekening = "";
            if (string.IsNullOrEmpty(rekening))
            {
                transaction.Status = "FAILED";
                return transaction;
            }
            else
            {

            }
            transaction.Status = "COMPLETED";
            return transaction;
        }
    }
}