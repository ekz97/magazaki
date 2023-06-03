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
                _contextService?.Logger?.LogInformation("TRX not specified");
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
                _contextService?.Logger?.LogWarning($"Source account found for {paymentTransaction.SourceInfo.Identifier}: saldo {fromAccount.Saldo} (credit {fromAccount.KredietLimiet})");
            }
            catch(Exception fromEx)
            {
                _contextService?.Logger?.LogWarning($"Source account not found for {paymentTransaction.SourceInfo.Identifier}: {fromEx.Message}");
 
                var addressManager = new AdresManager(new AdresRepository(connectionString));
                Adres? address = null;
                try
                {
                    _contextService?.Logger?.LogDebug($"Fetching address...");
                    address = addressManager.AdresOphalenAsync("ST", "01", "0000", "GEM", "BE").Result;
                }
                catch(Exception addressEx)
                {
                    _contextService?.Logger?.LogWarning($"Exception fetching address: {addressEx.Message}");
                }
                if (address == null)
                {
                    _contextService?.Logger?.LogDebug($"Creating address...");
                    address = new(Guid.NewGuid(), "ST", "01", "0000", "GEM", "BE");
                    addressManager.AdresToevoegenAsync(address).Wait();
                }
                var userManager = new GebruikerManager(new GebruikerRepository(connectionString));
                Gebruiker? user = null;
                try
                {
                    _contextService?.Logger?.LogDebug($"Fetching user {paymentTransaction.SourceInfo.Identifier}...");
                    user = userManager.GebruikerOphalenAsync(paymentTransaction.SourceInfo.Identifier).Result;
                }
                catch(Exception userEx)
                {
                    _contextService?.Logger?.LogWarning($"Exception fetching user: {userEx.Message}");
                }
                if(user == null)
                {
                    _contextService?.Logger?.LogDebug($"Adding user...");
                    user = new Gebruiker(Guid.NewGuid(), "FN", "VN", paymentTransaction.SourceInfo.Identifier, "+32474437788", "CODE", "03/06/1980", address);
                    userManager.GebruikerToevoegenAsync(user).Wait();
                }
                _contextService?.Logger?.LogDebug($"Adding account...");
                fromAccount = new Rekening(Guid.NewGuid(), "BE68539007547034", RekeningType.Zichtrekening, 5000, 5000, new List<Transactie>(), user);
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
                _contextService?.Logger?.LogWarning($"Destination account not found for {paymentTransaction.DestinationInfo.Identifier}: {toEx.Message}");

                var addressManager = new AdresManager(new AdresRepository(connectionString));
                Adres? address = null;
                try
                {
                    _contextService?.Logger?.LogDebug($"Fetching address...");
                    address = addressManager.AdresOphalenAsync("ST", "01", "0000", "GEM", "BE").Result;
                }
                catch (Exception addressEx)
                {
                    _contextService?.Logger?.LogWarning($"Exception fetching address: {addressEx.Message}");
                }
                if(address == null)
                {
                    _contextService?.Logger?.LogDebug($"Adress not found, creating...");
                    address = new(Guid.NewGuid(), "ST", "01", "0000", "GEM", "BE");
                    addressManager.AdresToevoegenAsync(address).Wait();
                    _contextService?.Logger?.LogDebug($"Address added");
                }
                var userManager = new GebruikerManager(new GebruikerRepository(connectionString));
                Gebruiker? user = null;
                try
                {
                    _contextService?.Logger?.LogDebug($"Fetching user...");
                    user = userManager.GebruikerOphalenAsync("luc.vervoort@hogent.be").Result;
                }
                catch (Exception userEx)
                {
                    _contextService?.Logger?.LogWarning($"Exception fetching user: {userEx.Message}");
                }
                if (user == null)
                {
                    _contextService?.Logger?.LogDebug($"Creating user...");
                    user = new Gebruiker(Guid.NewGuid(), "FN", "VN", "luc.vervoort@hogent.be", "+32474437788", "CODE", "03/06/1980", address);
                    userManager.GebruikerToevoegenAsync(user).Wait();
                }
                _contextService?.Logger?.LogDebug($"Creating account...");
                toAccount = new Rekening(Guid.NewGuid(), paymentTransaction.DestinationInfo.Identifier, RekeningType.Zichtrekening, 5000, 5000, new List<Transactie>(), user);
                rekeningManager.RekeningToevoegenAsync(toAccount).Wait();
            }

            if (fromAccount == null || toAccount == null)
            {
                paymentTransaction.Status = "FAILED";
                _contextService?.Logger?.LogError("Account not found");
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
                    _contextService?.Logger?.LogError("Funds transfer failed");
                    _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process (TRANSFER FAILED)");
                    return paymentTransaction;
                }
            }
            paymentTransaction.Status = "COMPLETED";
            _contextService?.Logger?.LogInformation("Funds transfer success");
            _contextService?.Logger?.LogDebug("<- PaymentTransactionService::Process");
            return paymentTransaction;
        }
    }
}