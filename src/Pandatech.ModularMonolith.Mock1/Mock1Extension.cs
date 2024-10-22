using MassTransit.PostgresOutbox.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Pandatech.ModularMonolith.Mock1.Context;
using Pandatech.ModularMonolith.Mock1.Helpers;
using Pandatech.ModularMonolith.SharedKernel.Extensions;
using Pandatech.ModularMonolith.SharedKernel.Helpers;

namespace Pandatech.ModularMonolith.Mock1;

public static class Mock1Extension
{
   public static WebApplicationBuilder AddMock1Module(this WebApplicationBuilder builder)
   {
      AssemblyRegistry.AddAssemblies(typeof(Mock1Extension).Assembly);


      builder.AddPostgresContext<PostgresContext>(
         builder.Configuration.GetConnectionString(ConfigurationPaths.Postgres)!);
      builder.Services.AddOutboxInboxServices<PostgresContext>();

      builder.LogModuleRegistrationSuccess("Mock1");
      return builder;
   }

   public static WebApplication UseMock1Module(this WebApplication app)
   {
      app.LogModuleUseSuccess("Mock1");

      app.MigrateDatabase<PostgresContext>();

      return app;
   }
}