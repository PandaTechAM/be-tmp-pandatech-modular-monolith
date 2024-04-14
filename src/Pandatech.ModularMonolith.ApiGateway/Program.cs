using FinHub.ApiGateway.Extensions;
using FinHub.SharedKernel.Extensions;
using FinHub.SharedKernel.Helpers;
using FinHub.SharedKernel.SharedEndpoints;
using FluentMinimalApiMapper;
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
