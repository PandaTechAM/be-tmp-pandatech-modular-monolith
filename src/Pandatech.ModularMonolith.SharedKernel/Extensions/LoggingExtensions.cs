using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Pandatech.ModularMonolith.SharedKernel.Middlewares;

namespace Pandatech.ModularMonolith.SharedKernel.Extensions;

public static class LoggingExtensions
{
   public static WebApplication UseRequestResponseLogging(this WebApplication app)
   {
      if (app.Logger.IsEnabled(LogLevel.Information))
      {
         app.UseMiddleware<RequestResponseLoggingMiddleware>();
      }

      return app;
   }
}