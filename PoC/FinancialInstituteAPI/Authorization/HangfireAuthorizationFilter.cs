using Hangfire.Dashboard;

namespace FinancialInstituteAPI.Authorization
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