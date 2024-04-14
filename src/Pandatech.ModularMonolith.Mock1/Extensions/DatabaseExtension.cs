using EFCore.PostgresExtensions.Extensions;
using FinHub.Mock1.Context;
using FinHub.Mock1.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinHub.Mock1.Extensions;

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
