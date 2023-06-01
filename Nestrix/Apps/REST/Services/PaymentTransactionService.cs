using DataLayer.Repositories;
using LogicLayer.Managers;
using LogicLayer.Model;
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

            // var source = Guid.Parse("3c146461-8b97-4028-8a51-3511113d3e95");

            Rekening? fromAccount = null;
            try
            {
                fromAccount = rekeningManager.RekeningOphalenViaEmailAsync(transaction.SourceInfo.Identifier).Result;
                _contextService?.Logger?.LogDebug($"Source account found for {transaction.SourceInfo.Identifier}: saldo {fromAccount.Saldo} (credit {fromAccount.KredietLimiet}");
            }
            catch(Exception fromEx)
            {
                _contextService?.Logger?.LogDebug($"Source account not found: {fromEx.Message}");
                var userManager = new GebruikerManager(new GebruikerRepository(connectionString));
                var user = userManager.GebruikerOphalenAsync(transaction.SourceInfo.Identifier).Result;
                fromAccount = new Rekening(RekeningType.Zichtrekening, "BE68539007547034", 5000, user);
                rekeningManager.RekeningToevoegenAsync(fromAccount).Wait();
            }
            Rekening? toAccount = null;
            try
            {
                toAccount = rekeningManager.RekeningOphalenViaEmailAsync(transaction.DestinationInfo.Identifier).Result;
                _contextService?.Logger?.LogDebug($"Destination account found for {transaction.DestinationInfo.Identifier}");
            }
            catch (Exception toEx)
            {
                _contextService?.Logger?.LogDebug($"Destination account not found: {toEx.Message}");
            }

            // it is possible to send money to an account we do not manage as a bank ... we should verify the existence of the account at the destination bank however
            if (toAccount == null)
            {
                _contextService?.Logger?.LogDebug("Creating dummy account...");
                toAccount = new LogicLayer.Model.Rekening(LogicLayer.Model.RekeningType.Zichtrekening, "BE68539007547035", -1, null);
            }

            if (fromAccount == null || toAccount == null)
            {
                transaction.Status = "FAILED";
                _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process (ACT not found)");
                return transaction;
            }
            else
            {
                var success = false;
                try
                {
                    success = rekeningManager.TransferMoneyAsync(fromAccount, toAccount, transaction?.Amount?.Value ?? 0, transaction?.Comment).Result;
                }
                catch(Exception ex)
                {
                    _contextService?.Logger?.LogDebug(ex.Message);
                }
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