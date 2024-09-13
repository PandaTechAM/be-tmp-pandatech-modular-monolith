using System.Reflection;

namespace Pandatech.ModularMonolith.SharedKernel.Helpers;

public static class AssemblyRegistry
{
   private static readonly List<Assembly> Assemblies = [];

   public static void AddAssemblies(params Assembly[] assemblies)
   {
      lock (Assemblies)
      {
         foreach (var assembly in assemblies)
         {
            if (!Assemblies.Contains(assembly))
            {
               Assemblies.Add(assembly);
            }
         }
      }
   }

   public static void RemoveAllAssemblies()
   {
      lock (Assemblies)
      {
         Assemblies.Clear();
      }
   }

   public static IEnumerable<Assembly> GetAllAssemblies()
   {
      lock (Assemblies)
      {
         return Assemblies.ToArray();
      }
   }
}