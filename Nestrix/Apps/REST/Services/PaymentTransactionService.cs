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

        public PaymentTransactionDTO? Process(PaymentTransactionDTO transaction)
        {
            _contextService?.Logger?.LogDebug("-> PaymentTransactionService::Process");
            if (transaction == null) 
            {
                _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process (TRX NOT SPECIFIED)");
                return transaction;
            }
            string connectionString = _contextService?.Configuration?.GetConnectionString("PeasieAPIDB")!;
            _contextService?.Logger?.LogDebug($"Connection string: {connectionString}");
            var rekeningManager = new RekeningManager(new RekeningRepository(connectionString), new TransactieRepository(connectionString));

            _contextService?.Logger?.LogDebug($"TRX: {transaction.ToString()}");

            var source = Guid.Parse("3c146461-8b97-4028-8a51-3511113d3e95");

            var fromAccount = rekeningManager.RekeningOphalenAsync(Guid.Parse(transaction.SourceInfo.Identifier)).Result;
            var toAccount = rekeningManager.RekeningOphalenAsync(Guid.Parse(transaction.DestinationInfo.Identifier)).Result;


            if (fromAccount == null || toAccount == null)
            {
                transaction.Status = "FAILED";
                _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process (ACT not found)");
                return transaction;
            }
            else
            {
                var success = rekeningManager.TransferMoneyAsync(fromAccount, toAccount, transaction?.Amount?.Value?? 0).Result;
                if(!success)
                {
                    transaction.Status = "FAILED";
                    _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process (TRANSFER FAILED)");
                    return transaction;
                }
            }
            transaction.Status = "COMPLETED";
            _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process");
            return transaction;
        }
    }
}