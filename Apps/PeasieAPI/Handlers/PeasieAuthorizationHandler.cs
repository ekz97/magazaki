using Microsoft.AspNetCore.DataProtection;
using System.Data.Common;
using System.Data;
using PeasieLib.Models.DB;
using PeasieLib.Services;

namespace PeasieLib.Handlers;

public class PeasieAuthorizationHandler : AuthorizationHandler<PeasieAuthorizationRequirement>
{
    private readonly ILogger<PeasieAuthorizationHandler> _logger;
    private readonly IDataProtectionProvider _dataProtectionProvider;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PeasieAuthorizationHandler(ILogger<PeasieAuthorizationHandler> logger, IDataProtectionProvider dataProtectionProvider, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _dataProtectionProvider = dataProtectionProvider;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PeasieAuthorizationRequirement requirement)
    {
        var endpoint = _httpContextAccessor.HttpContext?.GetEndpoint();
        //var action = endpoint?.Metadata.OfType<ControllerActionDescriptor>().FirstOrDefault();
        //if (action != null)
        {
            // is the action is expecting a post DTO
            //if (!action.Parameters.Any(p => p.ParameterType == typeof(PeasieRequestDTO)))
            //{
            //    return Task.CompletedTask;
            //}

            //var request = _httpContextAccessor!.HttpContext!.Request.ReadAsJsonAsync<PeasieRequestDTO>().Result;
            //var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            //if (PeasieRequestDTO. == userId)
            //{
            //    context.Succeed(requirement);
            //}

            var result = EFSqlHelper.RawSqlQuery(new PeasieAPIDbContext(), "SELECT UserName, PasswordHash, Secret FROM peasieidentitydb.aspnetusers",
            x => new { EmailAddress = (string)x[0], Password = (string)x[1], Secret = (string)x[2] });

            // IDataProtector protector = _dataProtectionProvider.CreateProtector("APIKEY", new string[] { "lcvervoort@yahoo.com" });

            // TODO: retrieve the protected payload from the db

            // var unprotectedPayload = protector.Unprotect(protectedPayload);
            context.Succeed(requirement);
        }

        // ... let other handlers take a stab at this
        return Task.CompletedTask;
    }
}