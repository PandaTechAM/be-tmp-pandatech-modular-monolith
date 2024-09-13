using EFCore.PostgresExtensions.Extensions;
using EntityFramework.Exceptions.PostgreSQL;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Pandatech.ModularMonolith.SharedKernel.Extensions;

public static class DatabaseExtensions
{
   public static WebApplicationBuilder AddPostgresContext<TContext>(this WebApplicationBuilder builder,
      string connectionString)
      where TContext : DbContext
   {
      builder.Services.AddDbContextPool<TContext>(options =>
         options.UseNpgsql(connectionString)
                .UseQueryLocks()
                .UseExceptionProcessor()
                .UseSnakeCaseNamingConvention());
      return builder;
   }

   public static WebApplication MigrateDatabase<TContext>(this WebApplication app)
      where TContext : DbContext
   {
      using var scope = app.Services.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
      dbContext.Database.Migrate();
      return app;
   }
}