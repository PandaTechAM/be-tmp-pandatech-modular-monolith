using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Pandatech.ModularMonolith.SharedKernel.Behaviors;

namespace Pandatech.ModularMonolith.SharedKernel.Extensions;

public static class MediatrExtension
{
   public static WebApplicationBuilder AddMediatrWithBehaviors(this WebApplicationBuilder builder,
      IEnumerable<Assembly> assemblies)
   {
      builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies.ToArray()));
      builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
      builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviorWithoutResponse<,>));
      builder.Services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviorWithResponse<,>));
      builder.Services.AddValidatorsFromAssemblies(assemblies);
      return builder;
   }
}
