﻿using Elastic.CommonSchema.Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Pandatech.ModularMonolith.SharedKernel.Helpers;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace Pandatech.ModularMonolith.SharedKernel.Extensions;

public static class SerilogExtension
{
   public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
   {
      builder.Logging.ClearProviders();


      var loggerConfig = new LoggerConfiguration()
                         .FilterOutUnwantedLogs()
                         .Enrich
                         .FromLogContext()
                         .ConfigureEnvironmentSpecificSettings(builder)
                         .ReadFrom
                         .Configuration(builder.Configuration);

      Log.Logger = loggerConfig.CreateLogger();
      builder.Logging.AddSerilog();
      builder.Services.AddSingleton(Log.Logger);

      builder.Host.UseSerilog();
      return builder;
   }

   private static LoggerConfiguration ConfigureEnvironmentSpecificSettings(this LoggerConfiguration loggerConfig,
      WebApplicationBuilder builder)
   {
      if (builder.Environment.IsLocal())
      {
         loggerConfig
            .WriteTo
            .Console();
      }
      else if (builder.Environment.IsDevelopment())
      {
         loggerConfig
            .WriteTo
            .Console()
            .WriteTo
            .File(builder);
      }
      else
      {
         loggerConfig
            .WriteTo
            .File(builder);
      }

      return loggerConfig;
   }
   
   private static void File(this LoggerSinkConfiguration loggerConfig, WebApplicationBuilder builder)
   {
      loggerConfig
         .File(new EcsTextFormatter(),
            builder.GetLogsPath(),
            rollingInterval: RollingInterval.Day);
   }

   private static LoggerConfiguration FilterOutUnwantedLogs(this LoggerConfiguration loggerConfig)
   {
      loggerConfig
         .Filter
         .ByExcluding(logEvent => logEvent.ShouldExcludeHangfireDashboardLogs())
         .Filter
         .ByExcluding(logEvent => logEvent.ShouldExcludeOutboxDbCommandLogs())
         .Filter
         .ByExcluding(logEvent => logEvent.ShouldExcludeSwaggerLogs());
      return loggerConfig;
   }

   private static bool ShouldExcludeHangfireDashboardLogs(this LogEvent logEvent)
   {
      return logEvent.Properties.TryGetValue("RequestPath", out var requestPathValue)
             && requestPathValue is ScalarValue requestPath
             && requestPath
                .Value
                ?.ToString()
                ?.Contains("/hangfire") == true;
   }

   private static bool ShouldExcludeOutboxDbCommandLogs(this LogEvent logEvent)
   {
      var message = logEvent.RenderMessage();
      return message.Contains("outbox_messages") || message.Contains("OutboxMessages");
   }
   

   private static bool ShouldExcludeSwaggerLogs(this LogEvent logEvent)
   {
      return logEvent.Properties.TryGetValue("RequestPath", out var requestPathValue)
             && requestPathValue is ScalarValue requestPath
             && requestPath
                .Value
                ?.ToString()
                ?.Contains("/swagger") == true;
   }

   private static string GetLogsPath(this WebApplicationBuilder builder)
   {
      var persistencePath = builder.Configuration.GetPersistentPath();
      var repoName = builder.Configuration.GetRepositoryName();
      var envName = builder.Environment.GetShortEnvironmentName();
      var instanceId = Guid.NewGuid()
                           .ToString();
      var fileName = $"logs-{instanceId}-.json";
      return Path.Combine(persistencePath, repoName, envName, "logs", fileName);
   }
}