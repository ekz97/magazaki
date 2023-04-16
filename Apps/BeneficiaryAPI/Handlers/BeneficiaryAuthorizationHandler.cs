using BeneficiaryAPI.Authorization;
using BeneficiaryAPI.Context;
using Microsoft.AspNetCore.Authorization;

namespace BeneficiaryAPI.Handlers
{
    public class BeneficiaryAuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private readonly ILogger<BeneficiaryAuthorizationHandler> _logger;

        public BeneficiaryAuthorizationHandler(ILogger<BeneficiaryAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            _logger.LogDebug("-> BeneficiaryAuthorizationHandler::HandleRequirementAsync");
            /*
            try
            {
                using (var dbContext = new BeneficiaryAPIDbContext())
                {
                    using var dbContextTransaction = await dbContext.Database.BeginTransactionAsync();
                    // Add your authorization logic here.
                    //_logger.Info("Authenticating...");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FinancialInstituteAuthorizationHandler::HandleRequirementAsync");
                throw ex;
            }
            */
            _logger.LogDebug("<- BeneficiaryAuthorizationHandler::HandleRequirementAsync");
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}