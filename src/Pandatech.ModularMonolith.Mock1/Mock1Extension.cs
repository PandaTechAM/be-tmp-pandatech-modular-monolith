using MassTransit.PostgresOutbox.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Pandatech.ModularMonolith.Mock1.Context;
using Pandatech.ModularMonolith.Mock1.Helpers;
using SharedKernel.Helpers;
using SharedKernel.Logging;
using SharedKernel.Postgres.Extensions;

namespace Pandatech.ModularMonolith.Mock1;

public static class Mock1Extension
{
   public static WebApplicationBuilder AddMock1Module(this WebApplicationBuilder builder)
   {
      AssemblyRegistry.Add(typeof(Mock1Extension).Assembly);


      builder.AddPostgresContext<Mock1Context>(
         builder.Configuration.GetConnectionString(ConfigurationPaths.Postgres)!);
      builder.Services.AddOutboxInboxServices<Mock1Context>();

      builder.LogModuleRegistrationSuccess("Mock1");
      return builder;
   }

   public static WebApplication UseMock1Module(this WebApplication app)
   {
      app.LogModuleUseSuccess("Mock1");

      app.MigrateDatabase<Mock1Context>();

      return app;
   }
}