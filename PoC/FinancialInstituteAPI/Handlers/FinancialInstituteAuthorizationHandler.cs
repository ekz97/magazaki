using Microsoft.AspNetCore.Authorization;
using PeasieLib.Authorization;

namespace FinancialInstituteAPI.Handlers
{
    public class FinancialInstituteAuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        private readonly ILogger<FinancialInstituteAuthorizationHandler> _logger;

        public FinancialInstituteAuthorizationHandler(ILogger<FinancialInstituteAuthorizationHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            _logger.LogDebug("-> FinancialInstituteAuthorizationHandler::HandleRequirementAsync");
            /*
            try
            {
                using (var dbContext = new FinancialInstituteAPIDbContext())
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
            _logger.LogDebug("<- FinancialInstituteAuthorizationHandler::HandleRequirementAsync");
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}