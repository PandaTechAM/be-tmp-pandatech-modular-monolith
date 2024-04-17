using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Pandatech.ModularMonolith.SharedKernel.Helpers;

public class DatabaseHelper(IServiceProvider serviceProvider)
{
   public string ResetDatabase<T>() where T : DbContext
   {
      using var scope = serviceProvider.CreateScope();
      var dbContext = scope.ServiceProvider.GetRequiredService<T>();
      dbContext.Database.EnsureDeleted();
      dbContext.Database.Migrate();

      return "Database reset success!";
   }
}
