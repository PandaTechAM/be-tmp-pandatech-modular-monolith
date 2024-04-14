﻿using FinHub.SharedKernel.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

namespace FinHub.SharedKernel.Extensions;

public static class SerilogExtension
{
   public static WebApplicationBuilder AddSerilog(this WebApplicationBuilder builder)
   {
      var configuration = builder.Configuration;
      var indexName = configuration[ConfigurationPaths.ElasticIndex]!;
      var elasticSearchUrl = configuration.GetConnectionString(ConfigurationPaths.ElasticSearchUrl)!;

      var loggerConfig = new LoggerConfiguration()
         .Enrich.FromLogContext()
         .Enrich.WithMachineName()
         .Filter.ByExcluding(logEvent => logEvent.ShouldExcludeHangfireDashboardLogs())
         .ReadFrom.Configuration(configuration);

      ConfigureEnvironmentSpecificSettings(builder.Environment, loggerConfig, elasticSearchUrl, indexName);

      Log.Logger = loggerConfig.CreateLogger();
      builder.Logging.ClearProviders();
      builder.Logging.AddSerilog();
      builder.Services.AddSingleton(Log.Logger);
      return builder;
   }

   private static void ConfigureEnvironmentSpecificSettings(IHostEnvironment environment,
      LoggerConfiguration loggerConfig, string elasticSearchUrl, string indexName)
   {
      if (environment.IsLocal())
      {
         loggerConfig.WriteTo.Console();
      }

      else if (environment.IsDevelopment())

      {
         //loggerConfig.WriteTo.Console();
         ConfigureElasticsearch(loggerConfig, elasticSearchUrl, indexName, environment);
      }
      else
      {
         ConfigureElasticsearch(loggerConfig, elasticSearchUrl, indexName, environment);
      }
   }

   private static void ConfigureElasticsearch(LoggerConfiguration loggerConfig, string elasticSearchUrl,
      string indexName, IHostEnvironment environment)
   {
      var envName = environment.GetShortEnvironmentName();

      var formattedIndexName = !string.IsNullOrEmpty(envName)
         ? $"{indexName}-{envName}-logs-{DateTime.UtcNow:yyyy.MM}"
         : $"{indexName}-logs-{DateTime.UtcNow:yyyy.MM}";

      loggerConfig.WriteTo.Elasticsearch(elasticSearchUrl,
         formattedIndexName,
         autoRegisterTemplate: true,
         detectElasticsearchVersion: true,
         numberOfShards: 5,
         numberOfReplicas: 1,
         bufferBaseFilename: "./logs/elastic-buffer",
         bufferFileSizeLimitBytes: 1024 * 1024 * 16); // 16 MB each buffer file
   }

   private static bool ShouldExcludeHangfireDashboardLogs(this LogEvent logEvent)
   {
      return logEvent.Properties.TryGetValue("RequestPath", out var requestPathValue)
             && requestPathValue is ScalarValue requestPath
             && requestPath.Value?.ToString()?.Contains("/hangfire") == true;
   }
}
