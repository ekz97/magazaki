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
                _contextService?.Logger?.LogDebug($"Source account not found for {paymentTransaction.SourceInfo.Identifier}: {fromEx.Message}");
 
                var userManager = new GebruikerManager(new GebruikerRepository(connectionString));
                Gebruiker? user = null;
                try
                {
                    user = userManager.GebruikerOphalenAsync(paymentTransaction.SourceInfo.Identifier).Result;
                }
                catch(Exception userEx)
                {
                    user = new Gebruiker(Guid.NewGuid(), "FN", "VN", "luc.vervoort@hogent.be", "+32474437788", "CODE", "03/06/1980", new Adres(Guid.NewGuid(), "ST", "01", "0000", "GEM", "BE"));
                    userManager.GebruikerToevoegenAsync(user).Wait();
                }
                fromAccount = new Rekening(RekeningType.Zichtrekening, paymentTransaction.SourceInfo.Identifier, 5000, user);
                rekeningManager.RekeningToevoegenAsync(fromAccount).Wait();
            }
            Rekening? toAccount = null;
            try
            {
                toAccount = rekeningManager.RekeningOphalenViaIBANAsync(paymentTransaction.DestinationInfo.Identifier).Result;
                _contextService?.Logger?.LogDebug($"Destination account found for {paymentTransaction.DestinationInfo.Identifier}");
            }
            catch (Exception toEx)
            {
                _contextService?.Logger?.LogDebug($"Destination account not found for {paymentTransaction.DestinationInfo.Identifier}: {toEx.Message}");

                var userManager = new GebruikerManager(new GebruikerRepository(connectionString));
                Gebruiker? user = null;
                user = new Gebruiker(Guid.NewGuid(), "FN", "VN", "luc.vervoort@hogent.be", "+32474437788", "CODE", "03/06/1980", new Adres(Guid.NewGuid(), "ST", "01", "0000", "GEM", "BE"));
                userManager.GebruikerToevoegenAsync(user).Wait();
                toAccount = new Rekening(RekeningType.Zichtrekening, paymentTransaction.SourceInfo.Identifier, 5000, user);
                rekeningManager.RekeningToevoegenAsync(toAccount).Wait();
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