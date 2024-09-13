using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;
using Pandatech.ModularMonolith.SharedKernel.Helpers;
using Polly;

namespace Pandatech.ModularMonolith.SharedKernel.Extensions;

public static class ResilienceExtensions
{
   public static WebApplicationBuilder AddResilienceDefaultPipeline(this WebApplicationBuilder builder)
   {
      builder.Services.AddResiliencePipeline(ResilienceDefaultPipelineProvider.DefaultPipelineName,
         pipelineBuilder =>
         {
            pipelineBuilder.AddRetry(ResilienceDefaultPipelineProvider.DefaultNetworkRetryOptions)
                           .AddRetry(ResilienceDefaultPipelineProvider.TooManyRequestsRetryOptions)
                           .AddCircuitBreaker(ResilienceDefaultPipelineProvider.DefaultCircuitBreakerOptions)
                           .AddTimeout(TimeSpan.FromSeconds(8));
         });
      return builder;
   }
   
   public static IHttpResiliencePipelineBuilder AddResilienceDefaultPipeline(this IHttpClientBuilder builder)
   {
      return builder.AddResilienceHandler("DefaultPipeline",
         resilienceBuilder =>
         {
            resilienceBuilder.AddRetry(ResilienceHttpOptions.DefaultTooManyRequestsRetryOptions)
                             .AddRetry(ResilienceHttpOptions.DefaultNetworkRetryOptions)
                             .AddCircuitBreaker(ResilienceHttpOptions.DefaultCircuitBreakerOptions)
                             .AddTimeout(TimeSpan.FromSeconds(8));
         });
   }
}