using System.Reflection;

namespace FinHub.SharedKernel.Helpers;

public static class AssemblyRegistry
{
   private static readonly List<Assembly> _assemblies = [];

   public static void AddAssemblies(params Assembly[] assemblies)
   {
      lock (_assemblies)
      {
         foreach (var assembly in assemblies)
         {
            if (!_assemblies.Contains(assembly))
            {
               _assemblies.Add(assembly);
            }
         }
      }
   }

   public static void RemoveAllAssemblies()
   {
      lock (_assemblies)
      {
         _assemblies.Clear();
      }
   }

   public static IEnumerable<Assembly> GetAllAssemblies()
   {
      lock (_assemblies)
      {
         return _assemblies.ToArray();
      }
   }
}
