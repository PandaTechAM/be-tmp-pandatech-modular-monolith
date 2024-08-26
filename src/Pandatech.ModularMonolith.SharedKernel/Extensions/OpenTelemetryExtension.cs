using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Pandatech.ModularMonolith.SharedKernel.Extensions;

public static class OpenTelemetryExtension
{
   public static WebApplicationBuilder ConfigureOpenTelemetry(this WebApplicationBuilder builder)
   {
      builder.Logging.AddOpenTelemetry(x =>
      {
         x.IncludeScopes = true;
         x.IncludeFormattedMessage = true;
      });

      builder.Services
             .AddOpenTelemetry()
             .ConfigureResource(resource => resource.AddService(builder.Environment.ApplicationName))
             .WithMetrics(metrics =>
             {
                metrics.AddRuntimeInstrumentation()
                       .AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation()
                       .AddPrometheusExporter();
             })
             .WithTracing(tracing =>
             {
                tracing.AddAspNetCoreInstrumentation()
                       .AddHttpClientInstrumentation();
             });

      return builder;
   }
}