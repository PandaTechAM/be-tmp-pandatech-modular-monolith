using DistributedCache.Options;
using FluentMinimalApiMapper;
using Pandatech.Crypto.Extensions;
using Pandatech.ModularMonolith.ApiGateway.Extensions;
using Pandatech.ModularMonolith.SharedKernel.Extensions;
using ResponseCrafter.Enums;
using ResponseCrafter.Extensions;
using SharedKernel.Extensions;
using SharedKernel.Helpers;
using SharedKernel.Logging;
using SharedKernel.OpenApi;
using SharedKernel.Resilience;
using SharedKernel.ValidatorAndMediatR;

var builder = WebApplication.CreateBuilder(args);

builder.LogStartAttempt();
AssemblyRegistry.Add(typeof(Program).Assembly);

builder
   .ConfigureWithPandaVault()
   .AddOutboundLoggingHandler()
   .AddSerilog()
   .AddResponseCrafter(NamingConvention.ToSnakeCase)
   .AddOpenApi()
   .AddOpenTelemetry()
   .RegisterModules()
   .AddMinimalApis(AssemblyRegistry.ToArray())
   .AddControllers(AssemblyRegistry.ToArray())
   .AddMediatrWithBehaviors(AssemblyRegistry.ToArray())
   .AddMassTransit(AssemblyRegistry.ToArray())
   .AddResilienceDefaultPipeline()
   .AddRedis(KeyPrefix.AssemblyNamePrefix)
   .AddDistributedSignalR("DistributedSignalR")
   .MapDefaultTimeZone()
   .AddCors()
   .AddAes256Key(builder.Configuration.GetAesKey())
   .AddHealthChecks();


var app = builder.Build();

app
   .UseRequestLogging()
   .UseResponseCrafter()
   .UseCors()
   .MapMinimalApis()
   .MapHealthCheckEndpoints()
   .MapPrometheusExporterEndpoints()
   .UseModules()
   .EnsureHealthy()
   .ClearAssemblyRegistry()
   .UseOpenApi()
   .MapControllers();

app.LogStartSuccess();
app.Run();