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

        public PaymentTransactionDTO? Process(PaymentTransactionDTO paymentTransaction)
        {
            _contextService?.Logger?.LogDebug("-> PaymentTransactionService::Process");
            if (paymentTransaction == null) 
            {
                _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process (TRX NOT SPECIFIED)");
                return paymentTransaction;
            }
            string connectionString = _contextService?.Configuration?.GetConnectionString("PeasieAPIDB")!;
            _contextService?.Logger?.LogDebug($"Connection string: {connectionString}");

            var rekeningManager = new RekeningManager(new RekeningRepository(connectionString), new TransactieRepository(connectionString));

            _contextService?.Logger?.LogDebug($"TRX: {paymentTransaction.ToString()}");

            // var source = Guid.Parse("3c146461-8b97-4028-8a51-3511113d3e95");

            Rekening? fromAccount = null;
            try
            {
                fromAccount = rekeningManager.RekeningOphalenViaEmailAsync(paymentTransaction.SourceInfo.Identifier).Result;
                _contextService?.Logger?.LogDebug($"Source account found for {paymentTransaction.SourceInfo.Identifier}: saldo {fromAccount.Saldo} (credit {fromAccount.KredietLimiet})");
            }
            catch(Exception fromEx)
            {
                _contextService?.Logger?.LogDebug($"Source account not found: {fromEx.Message}");
                var userManager = new GebruikerManager(new GebruikerRepository(connectionString));
                var user = userManager.GebruikerOphalenAsync(paymentTransaction.SourceInfo.Identifier).Result;
                fromAccount = new Rekening(RekeningType.Zichtrekening, "BE68539007547034", 5000, user);
                rekeningManager.RekeningToevoegenAsync(fromAccount).Wait();
            }
            Rekening? toAccount = null;
            try
            {
                toAccount = rekeningManager.RekeningOphalenViaEmailAsync(paymentTransaction.DestinationInfo.Identifier).Result;
                _contextService?.Logger?.LogDebug($"Destination account found for {paymentTransaction.DestinationInfo.Identifier}");
            }
            catch (Exception toEx)
            {
                _contextService?.Logger?.LogDebug($"Destination account not found: {toEx.Message}");
            }

            // it is possible to send money to an account we do not manage as a bank ... we should verify the existence of the account at the destination bank however
            if (toAccount == null)
            {
                _contextService?.Logger?.LogDebug("Creating dummy account...");
                toAccount = new LogicLayer.Model.Rekening(LogicLayer.Model.RekeningType.Zichtrekening, paymentTransaction.DestinationInfo.Identifier, 5000, new Gebruiker(paymentTransaction.DestinationInfo.Type, "", "", "", DateTime.Now, new Adres(), Guid.NewGuid()));
            }

            if (fromAccount == null || toAccount == null)
            {
                paymentTransaction.Status = "FAILED";
                _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process (ACT not found)");
                return paymentTransaction;
            }
            else
            {
                var success = false;
                try
                {
                    success = rekeningManager.TransferMoneyAsync(fromAccount, toAccount, paymentTransaction?.Amount?.Value ?? 0, paymentTransaction?.Comment).Result;
                }
                catch(Exception ex)
                {
                    _contextService?.Logger?.LogDebug(ex.Message);
                }
                if(!success)
                {
                    paymentTransaction.Status = "FAILED";
                    _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process (TRANSFER FAILED)");
                    return paymentTransaction;
                }
            }
            paymentTransaction.Status = "COMPLETED";
            _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process");
            return paymentTransaction;
        }
    }
}