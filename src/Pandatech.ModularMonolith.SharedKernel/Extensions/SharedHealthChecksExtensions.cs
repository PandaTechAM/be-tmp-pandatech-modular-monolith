using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Pandatech.ModularMonolith.SharedKernel.Helpers;
using Pandatech.VerticalSlices.SharedKernel.Helpers;
using RabbitMQ.Client;

namespace Pandatech.ModularMonolith.SharedKernel.Extensions;

public static class SharedHealthChecksExtensions
{
   public static WebApplicationBuilder AddSharedHealthChecks(this WebApplicationBuilder builder)
   {
      var configuration = builder.Configuration;
      var timeoutSeconds = TimeSpan.FromSeconds(5);
      var redisConnectionString = configuration.GetConnectionString(builder.Configuration.GetRedisUrl())!;
      var rabbitMqUri = configuration.GetConnectionString(builder.Configuration.GetRabbitMqUrl())!;

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
                .AddRabbitMQ();
      }
      else
      {
         builder.Services
                .AddSingleton(connection)
                .AddHealthChecks()
                .AddRedis(redisConnectionString, timeout: timeoutSeconds)
                .AddRabbitMQ();
      }

      return builder;
   }
}