using FinHub.ApiGateway.Extensions;
using FluentMinimalApiMapper;
using Pandatech.ModularMonolith.SharedKernel.Extensions;
using Pandatech.ModularMonolith.SharedKernel.Helpers;
using Pandatech.ModularMonolith.SharedKernel.SharedEndpoints;
using PandaVaultClient;
using ResponseCrafter;

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
   .AddMassTransit(AssemblyRegistry.GetAllAssemblies().ToArray())
   .ConfigureOpenTelemetry();

builder.Services
   .AddSwaggerGen()
   .AddEndpointsApiExplorer();


var app = builder.Build();

app.UseStaticFiles();

app.UseResponseCrafter()
   .UseCors()
   .UseModules()
   .EnsureHealthy()
   .UseSwagger(app.Configuration);

app.MapPandaEndpoints();
app.MapEndpoints();

StartupLogger.LogStartSuccess();
app.Run();

namespace Pandatech.ModularMonolith.ApiGateway
{
   public partial class Program;
}
