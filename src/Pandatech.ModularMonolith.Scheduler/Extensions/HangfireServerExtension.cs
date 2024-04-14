using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Pandatech.ModularMonolith.Scheduler.Helpers;

namespace Pandatech.ModularMonolith.Scheduler.Extensions;

public static class HangfireServerExtension
{
   public static WebApplicationBuilder AddHangfireServer(this WebApplicationBuilder builder)
   {
      var postgresConnectionString = builder.Configuration.GetConnectionString(ConfigurationPaths.Postgres);
      builder.Services.AddHangfire(configuration =>
      {
         configuration.SetDataCompatibilityLevel(CompatibilityLevel.Version_180);
         configuration.UseSimpleAssemblyNameTypeSerializer();
         configuration.UseRecommendedSerializerSettings();
         configuration.UsePostgreSqlStorage(c => c.UseNpgsqlConnection(postgresConnectionString));
      });

      builder.Services.AddHangfireServer(options =>
      {
         //options.WorkerCount = 10;
      });
      return builder;
   }
}
