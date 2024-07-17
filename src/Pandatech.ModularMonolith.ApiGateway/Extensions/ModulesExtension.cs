using FluentMinimalApiMapper;
using Pandatech.ModularMonolith.Mock1;
using Pandatech.ModularMonolith.Mock2;
using Pandatech.ModularMonolith.Scheduler;
using Pandatech.ModularMonolith.SharedKernel.Extensions;
using Pandatech.ModularMonolith.SharedKernel.Helpers;

namespace Pandatech.ModularMonolith.ApiGateway.Extensions;

public static class ModulesExtension
{
   public static WebApplicationBuilder RegisterModules(this WebApplicationBuilder builder)
   {
      if (!builder.Environment.IsProduction())
      {
         builder.AddMock1Module();
         builder.AddMock2Module();
      }

      builder.AddSchedulerModule();

      var assemblies = AssemblyRegistry.GetAllAssemblies()
                                       .ToList();
      builder
         .AddEndpoints(assemblies) //After modules always
         .AddMediatrWithBehaviors(assemblies); //After modules always

      return builder;
   }

   public static WebApplication UseModules(this WebApplication app)
   {
      if (!app.Environment.IsProduction())
      {
         app.UseMock1Module();
         app.UseMock2Module();
      }

      app.UseSchedulerModule();
      return app;
   }
}