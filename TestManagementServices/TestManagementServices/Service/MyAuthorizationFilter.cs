using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace TestManagementServices.Service
{
    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}