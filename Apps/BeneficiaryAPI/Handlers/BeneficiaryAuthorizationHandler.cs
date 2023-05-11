using BeneficiaryAPI.Context;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using PeasieLib.Authorization;
using System.Data.Common;
using System.Data;

namespace BeneficiaryAPI.Handlers
{
    public class IdentityUser
    {
        public string? EmailAddress { get; set; }
        public string? Secret { get; set; }
        public string? Password { get; set; }
    }
    
    public static class EFSqlHelper
    {
        public static List<T> RawSqlQuery<T>(string query, Func<DbDataReader, T> map)
        {
            using (var context = new BeneficiaryAPIDbContext())
            {
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = query;
                    command.CommandType = CommandType.Text;

                    context.Database.OpenConnection();

                    using (var result = command.ExecuteReader())
                    {
                        var entities = new List<T>();

                        while (result.Read())
                        {
                            entities.Add(map(result));
                        }

                        return entities;
                    }
                }
            }
        }
    }

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
                    _logger.LogDebug("Authorizing...");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in FinancialInstituteAuthorizationHandler::HandleRequirementAsync");
            }
            */
            var result = EFSqlHelper.RawSqlQuery("SELECT UserName, PasswordHash, Secret FROM aspnetusers",
                    x => new IdentityUser { EmailAddress = (string)x[0], Password = (string)x[1], Secret = (string)x[2] });

            _logger.LogDebug("<- BeneficiaryAuthorizationHandler::HandleRequirementAsync");
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}