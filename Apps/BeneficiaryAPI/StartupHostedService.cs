namespace BeneficiaryAPI
{
    public class StartupHostedService : IHostedService
    {
        private readonly ILogger<StartupHostedService> _logger;

        public StartupHostedService(ILogger<StartupHostedService> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken stoppingToken)
        {
            _logger?.LogDebug("-> StartupHostedService::StartAsync");
            // The code in here will run when the application starts, and block the startup process until finished
            Program.EveryMinute();
            _logger?.LogDebug("<- StartupHostedService::StartAsync");
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            // The code in here will run when the application stops
            // In your case, nothing to do
            return Task.CompletedTask;
        }
    }
}