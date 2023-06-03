using Hangfire.Dashboard;

namespace BeneficiaryAPI.Authorization
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