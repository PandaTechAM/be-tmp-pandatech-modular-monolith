using System.Reflection;

namespace Pandatech.ModularMonolith.E2ETests.Dtos;

public class Project(string groupName, ProjectType type, Assembly assembly)
{
   public string GroupName { get; private init; } = groupName;

   public ProjectType Type { get; set; } = type;

   public Assembly Assembly { get; } = assembly;

   public string? AssemblyName =>
      Assembly.GetName()
              .Name;
}

public enum ProjectType
{
   Module,
   ModuleIntegration,
   ApiGateway,
   SharedKernel
}