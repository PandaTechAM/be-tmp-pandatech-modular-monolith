using MassTransit.PostgresOutbox.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Pandatech.ModularMonolith.Mock2.Context;
using Pandatech.ModularMonolith.Mock2.Helpers;
using SharedKernel.Helpers;
using SharedKernel.Logging;
using SharedKernel.Postgres.Extensions;

namespace Pandatech.ModularMonolith.Mock2;

public static class Mock2Extension
{
   public static WebApplicationBuilder AddMock2Module(this WebApplicationBuilder builder)
   {
      AssemblyRegistry.Add(typeof(Mock2Extension).Assembly);


      builder.AddPostgresContextPool<Mock2Context>(
         builder.Configuration.GetConnectionString(ConfigurationPaths.Postgres)!);
      builder.Services.AddOutboxInboxServices<Mock2Context>();

      builder.LogModuleRegistrationSuccess("Mock2");
      return builder;
   }

   public static WebApplication UseMock2Module(this WebApplication app)
   {
      app.MigrateDatabase<Mock2Context>();
      app.LogModuleUseSuccess("Mock2");
      return app;
   }
}