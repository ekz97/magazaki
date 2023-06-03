using Hangfire.Dashboard;

namespace BankingRestAPI.Authorization
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {

        public HangfireAuthorizationFilter()
        {
        }

        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}