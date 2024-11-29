using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Pandatech.ModularMonolith.Scheduler.Context;

public class SchedulerContextFactory : IDesignTimeDbContextFactory<SchedulerContext>
{
   public SchedulerContext CreateDbContext(string[] args)
   {
      var optionsBuilder = new DbContextOptionsBuilder<SchedulerContext>();

      optionsBuilder.UseNpgsql()
                    .UseSnakeCaseNamingConvention();

      return new SchedulerContext(optionsBuilder.Options);
   }
}