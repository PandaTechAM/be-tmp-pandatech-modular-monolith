using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pandatech.ModularMonolith.Mock1.Context;

public class Mock1ContextFactory : IDesignTimeDbContextFactory<Mock1Context>
{
   public Mock1Context CreateDbContext(string[] args)
   {
      var optionsBuilder = new DbContextOptionsBuilder<Mock1Context>();

      optionsBuilder.UseNpgsql()
                    .UseSnakeCaseNamingConvention();

      return new Mock1Context(optionsBuilder.Options);
   }
}