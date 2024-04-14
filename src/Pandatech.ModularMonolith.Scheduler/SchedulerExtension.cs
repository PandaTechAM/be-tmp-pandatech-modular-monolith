using FinHub.Scheduler.Contracts;
using Pandatech.ModularMonolith.Scheduler.Extensions;
using FinHub.SharedKernel.Extensions;
using FinHub.SharedKernel.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pandatech.ModularMonolith.Scheduler.Context;
using Pandatech.ModularMonolith.Scheduler.Helpers;
using Pandatech.ModularMonolith.Scheduler.Services;

namespace Pandatech.ModularMonolith.Scheduler;

public static class SchedulerExtension
{
   public static WebApplicationBuilder AddSchedulerModule(this WebApplicationBuilder builder)
   {
      AssemblyRegistry.AddAssemblies(typeof(SchedulerExtension).Assembly);

      builder
         .AddPostgresContext<PostgresContext>(builder.Configuration.GetConnectionString(ConfigurationPaths.Postgres)!)
         .AddHangfireServer()
         .AddHealthChecks();

      builder.Services.AddSingleton<IBackgroundJob, BackgroundJob>();

      StartupLogger.LogModuleRegistrationSuccess("Scheduler");

      return builder;
   }

   public static WebApplication UseSchedulerModule(this WebApplication app)
   {
      app.MigrateDatabase<PostgresContext>()
         .RunHangfireDashboard();

      StartupLogger.LogModuleUseSuccess("Scheduler");
      return app;
   }
}
