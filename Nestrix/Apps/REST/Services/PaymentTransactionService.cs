using DataLayer.Repositories;
using LogicLayer.Managers;
using Peasie.Contracts;
using RESTLayer.Interfaces;

namespace RESTLayer.Services
{
    public class PaymentTransactionService : IPaymentTransactionService
    {
        private RekeningManager? _rekeningManager = null;
        private readonly ILogger<PaymentTransactionService> _logger;
        private readonly IConfiguration _configuration;

        public PaymentTransactionService(ILogger<PaymentTransactionService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }   

        public PaymentTransactionDTO Process(PaymentTransactionDTO transaction)
        {
            _logger?.LogDebug("-> PaymentTransactionService::Process");
            string connectionString = _configuration.GetConnectionString("PeasieAPIDB")!;
            _rekeningManager = new(new RekeningRepository(connectionString), new TransactieRepository(connectionString));
            var source = Guid.Parse("3c146461-8b97-4028-8a51-3511113d3e95");
            var rekening = "";
            if (string.IsNullOrEmpty(rekening))
            {
                transaction.Status = "FAILED";
                _logger?.LogDebug("<- PaymentTransactionService::Process (FAILED)");
                return transaction;
            }
            else
            {

            }
            transaction.Status = "COMPLETED";
            _logger?.LogDebug("<- PaymentTransactionService::Process");
            return transaction;
        }
    }
}