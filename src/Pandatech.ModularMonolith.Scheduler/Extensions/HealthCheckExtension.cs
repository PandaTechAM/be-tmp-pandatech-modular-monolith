using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pandatech.ModularMonolith.Scheduler.Helpers;

namespace Pandatech.ModularMonolith.Scheduler.Extensions;

public static class HealthCheckExtension
{
   public static WebApplicationBuilder AddHealthChecks(this WebApplicationBuilder builder)
   {
      var configuration = builder.Configuration;
      var timeoutSeconds = TimeSpan.FromSeconds(5);
      var postgresConnectionString = configuration.GetConnectionString(ConfigurationPaths.Postgres)!;

      builder.Services
         .AddHealthChecks()
         .AddNpgSql(postgresConnectionString, timeout: timeoutSeconds, name: "postgres_pandatech_modular_monolith_scheduler");

      return builder;
   }
}
