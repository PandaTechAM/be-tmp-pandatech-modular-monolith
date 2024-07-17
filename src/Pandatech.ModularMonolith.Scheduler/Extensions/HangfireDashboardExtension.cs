using Hangfire;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Pandatech.ModularMonolith.Scheduler.Helpers;

namespace Pandatech.ModularMonolith.Scheduler.Extensions;

public static class HangfireDashboardExtension
{
   public static WebApplication RunHangfireDashboard(this WebApplication app)
   {
      var user = app.Configuration[ConfigurationPaths.HangfireDashboardUser];
      var pass = app.Configuration[ConfigurationPaths.HangfireDashboardPassword];

      app.UseHangfireDashboard("/hangfire",
         new DashboardOptions
         {
            DashboardTitle = "Scheduler Dashboard",
            Authorization = new[]
            {
               new HangfireCustomBasicAuthenticationFilter
               {
                  User = user,
                  Pass = pass
               }
            }
         });
      app.MapHangfireDashboard();

      return app;
   }
}