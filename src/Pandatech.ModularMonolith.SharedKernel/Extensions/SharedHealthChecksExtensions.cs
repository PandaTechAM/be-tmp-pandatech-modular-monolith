using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pandatech.ModularMonolith.SharedKernel.Helpers;
using RabbitMQ.Client;

namespace Pandatech.ModularMonolith.SharedKernel.Extensions;

public static class SharedHealthChecksExtensions
{
   public static WebApplicationBuilder AddSharedHealthChecks(this WebApplicationBuilder builder)
   {
      var configuration = builder.Configuration;
      var timeoutSeconds = TimeSpan.FromSeconds(5);
      var redisConnectionString = configuration.GetConnectionString(ConfigurationPaths.RedisUrl)!;
      var elasticSearchUrl = configuration.GetConnectionString(ConfigurationPaths.ElasticSearchUrl)!;
      var rabbitMqUri = configuration.GetConnectionString(ConfigurationPaths.RabbitMqUrl)!;

      //This part is only for RMQ health check
      ConnectionFactory factory = new()
      {
         Uri = new Uri(rabbitMqUri)
      };
      var connection = factory.CreateConnection();


      if (builder.Environment.IsLocal())
      {
         builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddRabbitMQ(name: "rabbit_mq")
                .AddRedis(redisConnectionString, timeout: timeoutSeconds);
      }

      else if (builder.Environment.IsProduction())
      {
         builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddRedis(redisConnectionString, timeout: timeoutSeconds)
                .AddElasticsearch(elasticSearchUrl, timeout: timeoutSeconds)
                .AddRabbitMQ();
      }
      else
      {
         builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddRedis(redisConnectionString, timeout: timeoutSeconds)
                .AddElasticsearch(elasticSearchUrl, timeout: timeoutSeconds)
                .AddRabbitMQ();
      }

      return builder;
   }
}