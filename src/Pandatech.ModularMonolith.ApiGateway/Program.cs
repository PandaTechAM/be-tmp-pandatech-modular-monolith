using FluentMinimalApiMapper;
using Pandatech.ModularMonolith.ApiGateway.Extensions;
using Pandatech.ModularMonolith.SharedKernel.Extensions;
using Pandatech.ModularMonolith.SharedKernel.Helpers;
using Pandatech.ModularMonolith.SharedKernel.SharedEndpoints;
using PandaVaultClient;
using ResponseCrafter.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.LogStartAttempt();
AssemblyRegistry.AddAssemblies(typeof(Program).Assembly);

if (!builder.Environment.IsLocal())
{
   builder.Configuration.AddPandaVault();
}

builder
   .AddResponseCrafter()
   .AddSerilog()
   .RegisterModules()
   .AddCors()
   .AddSwagger()
   .AddPandaCryptoAndFilters()
   .AddSharedHealthChecks()
   .AddResilienceDefaultPipeline()
   .AddMassTransit(AssemblyRegistry.GetAllAssemblies()
                                   .ToArray())
   .ConfigureOpenTelemetry();

builder.Services
       .AddSwaggerGen()
       .AddEndpointsApiExplorer();


var app = builder.Build();

app.UseStaticFiles();

app
   .UseRequestResponseLogging()
   .UseResponseCrafter()
   .UseCors()
   .UseModules()
   .EnsureHealthy()
   .UseSwagger(app.Configuration);

app.MapPandaEndpoints();
app.MapEndpoints();

app.LogStartSuccess();
app.Run();

namespace Pandatech.ModularMonolith.ApiGateway
{
   public class Program;
}