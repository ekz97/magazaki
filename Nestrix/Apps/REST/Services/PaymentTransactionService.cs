using DataLayer.Repositories;
using LogicLayer.Managers;
using Peasie.Contracts;
using PeasieLib.Interfaces;
using RESTLayer.Interfaces;

namespace RESTLayer.Services
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private readonly IPeasieApplicationContextService _contextService;

        public PaymentTransactionService(IPeasieApplicationContextService applicationContextService)
        {
            _contextService = applicationContextService;
        }   

        public PaymentTransactionDTO Process(PaymentTransactionDTO transaction)
        {
            _contextService?.Logger?.LogDebug("-> PaymentTransactionService::Process");
            string connectionString = _contextService?.Configuration?.GetConnectionString("PeasieAPIDB")!;
            var rekeningManager = new RekeningManager(new RekeningRepository(connectionString), new TransactieRepository(connectionString));
            var source = Guid.Parse("3c146461-8b97-4028-8a51-3511113d3e95");
            var rekening = "";
            if (string.IsNullOrEmpty(rekening))
            {
                transaction.Status = "FAILED";
                _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process (FAILED)");
                return transaction;
            }
            else
            {

            }
            transaction.Status = "COMPLETED";
            _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process");
            return transaction;
        }
    }
}