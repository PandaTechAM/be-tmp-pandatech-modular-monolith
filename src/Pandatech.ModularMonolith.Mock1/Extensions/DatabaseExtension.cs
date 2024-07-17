using EFCore.PostgresExtensions.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pandatech.ModularMonolith.Mock1.Context;
using Pandatech.ModularMonolith.Mock1.Helpers;

namespace Pandatech.ModularMonolith.Mock1.Extensions;

public static class DatabaseExtensions
{
   public static WebApplicationBuilder AddPostgresContext(this WebApplicationBuilder builder)
   {
      var configuration = builder.Configuration;

      var connectionString = configuration.GetConnectionString(ConfigurationPaths.Postgres);
      builder.Services.AddDbContextPool<PostgresContext>(options =>
         options.UseNpgsql(connectionString)
                .UseQueryLocks()
                .UseSnakeCaseNamingConvention());
      return builder;
   }

   public static WebApplication MigrateDatabase(this WebApplication app)
   {
      using var scope = app.Services.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<PostgresContext>();
      dbContext.Database.Migrate();
      return app;
   }
}