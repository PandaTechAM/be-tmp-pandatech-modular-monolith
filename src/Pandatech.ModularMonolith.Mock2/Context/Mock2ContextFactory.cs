using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pandatech.ModularMonolith.Mock2.Context;

public class Mock2ContextFactory : IDesignTimeDbContextFactory<Mock2Context>
{
   public Mock2Context CreateDbContext(string[] args)
   {
      var optionsBuilder = new DbContextOptionsBuilder<Mock2Context>();

      optionsBuilder.UseNpgsql()
                    .UseSnakeCaseNamingConvention();

      return new Mock2Context(optionsBuilder.Options);
   }
}