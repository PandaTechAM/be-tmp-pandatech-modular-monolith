using MassTransit.PostgresOutbox.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Pandatech.ModularMonolith.Mock2.Context;
using Pandatech.ModularMonolith.Mock2.Helpers;
using Pandatech.ModularMonolith.SharedKernel.Extensions;
using Pandatech.ModularMonolith.SharedKernel.Helpers;

namespace Pandatech.ModularMonolith.Mock2;

public static class Mock2Extension
{
   public static WebApplicationBuilder AddMock2Module(this WebApplicationBuilder builder)
   {
      AssemblyRegistry.AddAssemblies(typeof(Mock2Extension).Assembly);


      builder.AddPostgresContext<PostgresContext>(
         builder.Configuration.GetConnectionString(ConfigurationPaths.Postgres)!);
      builder.Services.AddOutboxInboxServices<PostgresContext>();

      StartupLogger.LogModuleRegistrationSuccess("Mock2");
      return builder;
   }

   public static WebApplication UseMock2Module(this WebApplication app)
   {
      app.MigrateDatabase<PostgresContext>();
      StartupLogger.LogModuleUseSuccess("Mock2");
      return app;
   }
}
