using Microsoft.AspNetCore.DataProtection;

namespace PeasieLib.Handlers;

public class PeasieAuthorizationHandler : AuthorizationHandler<PeasieAuthorizationRequirement>
{
    private readonly ILogger<PeasieAuthorizationHandler> _logger;
    private readonly IDataProtectionProvider _dataProtectionProvider;

    public PeasieAuthorizationHandler(ILogger<PeasieAuthorizationHandler> logger, IDataProtectionProvider dataProtectionProvider)
    {
        _logger = logger;
        _dataProtectionProvider = dataProtectionProvider;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PeasieAuthorizationRequirement requirement)
    {
        /*
        using (var dbContext = new PeasieAPIDbContext())
        {
            using var dbContextTransaction = await dbContext.Database.BeginTransactionAsync();
            // Add your authorization logic here.
            //_logger.Info("Authorizing...");
        }
        */

        IDataProtector protector = _dataProtectionProvider.CreateProtector("APIKEY", new string[] { "lcvervoort@yahoo.com" });

        // TODO: retrieve the protected payload from the db

        // var unprotectedPayload = protector.Unprotect(protectedPayload);

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}