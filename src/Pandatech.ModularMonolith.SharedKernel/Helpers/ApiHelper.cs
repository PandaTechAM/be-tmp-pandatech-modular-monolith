namespace Pandatech.ModularMonolith.SharedKernel.Helpers;

public static class ApiHelper
{
   private const string BaseApiPath = "/api/v";

   public const string GroupNameModular = "modular-v1";


   public static string GetRoutePrefix(int version, string baseRoute)
   {
      return $"{BaseApiPath}{version}{baseRoute}";
   }
}