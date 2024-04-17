using System.Net;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Pandatech.ModularMonolith.SharedKernel.Helpers;
using PandaVaultClient;

namespace Pandatech.ModularMonolith.SharedKernel.SharedEndpoints;

public static class PandaEndpoints
{
   private const string TagName = "above-board";
   private const string BasePath = $"/{TagName}";

   public static void MapPandaEndpoints(this WebApplication app)
   {
      if (!app.Environment.IsProduction())
      {
         app.MapPandaVaultApi($"{BasePath}/configuration", TagName, ApiHelper.GroupNameMain);
      }


      app.MapPingEndpoint(true)
         .MapHealthEndpoint(true)
         .MapPrometheusEndpoints(true);
   }

   private static WebApplication MapPrometheusEndpoints(this WebApplication app, bool enabled)
   {
      if (!enabled)
      {
         return app;
      }

      app.MapPrometheusScrapingEndpoint($"{BasePath}/metrics");

      app.UseHealthChecksPrometheusExporter($"{BasePath}/metrics/health",
         options => options.ResultStatusCodes[HealthStatus.Unhealthy] = (int)HttpStatusCode.OK);

      return app;
   }

   private static WebApplication MapPingEndpoint(this WebApplication app, bool enabled)
   {
      if (!enabled)
      {
         return app;
      }

      app.MapGet($"{BasePath}/ping", () => "pong")
         .Produces<string>()
         .WithTags(TagName)
         .WithGroupName(ApiHelper.GroupNameMain)
         .WithOpenApi();
      return app;
   }

   private static WebApplication MapHealthEndpoint(this WebApplication app, bool enabled)
   {
      if (!enabled)
      {
         return app;
      }

      app.MapHealthChecks($"{BasePath}/health",
         new HealthCheckOptions { ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse });

      return app;
   }
}
