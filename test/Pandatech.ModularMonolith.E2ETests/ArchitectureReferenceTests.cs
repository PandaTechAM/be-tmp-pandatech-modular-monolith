using FinHub.E2ETests.Dtos;
using NetArchTest.Rules;
using Pandatech.ModularMonolith.ApiGateway;
using Pandatech.ModularMonolith.Mock1;
using Pandatech.ModularMonolith.Mock1.Integration;
using Pandatech.ModularMonolith.Mock2;
using Pandatech.ModularMonolith.Scheduler;

namespace FinHub.E2ETests;

public class ArchitectureReferenceTests
{
   private static readonly List<Project> Projects =
   [
      new Project("Mock1", ProjectType.Module, typeof(Mock1Extension).Assembly),
      new Project("Mock1Integration",
         ProjectType.ModuleIntegration,
         typeof(AssemblyReference).Assembly),

      new Project("Mock2", ProjectType.Module, typeof(Mock2Extension).Assembly),
      new Project("Mock2Integration",
         ProjectType.ModuleIntegration,
         typeof(Pandatech.ModularMonolith.Mock2.Integration.AssemblyReference).Assembly),

      new Project("Scheduler", ProjectType.Module, typeof(SchedulerExtension).Assembly),
      new Project("SchedulerIntegration",
         ProjectType.ModuleIntegration,
         typeof(Pandatech.ModularMonolith.Scheduler.Integration.AssemblyReference).Assembly),

      new Project("ApiGateway", ProjectType.ApiGateway, typeof(Program).Assembly),
      new Project("SharedKernel",
         ProjectType.SharedKernel,
         typeof(Pandatech.ModularMonolith.SharedKernel.AssemblyReference).Assembly)
   ];

   [Fact]
   public void ApiGateway_Should_HaveDependency_On_All_Modules()
   {
      var apiGateway = Projects.First(p => p.Type == ProjectType.ApiGateway);
      var modules = Projects.Where(p => p.Type == ProjectType.Module);
      var referencedAssemblies = apiGateway.Assembly.GetReferencedAssemblies();

      foreach (var module in modules)
      {
         var hasReference = referencedAssemblies.Any(s => s.Name == module.AssemblyName);

         Assert.True(hasReference,
            $"module should have dependency on All Modules" +
            $"Group name: {module.GroupName}" +
            $"Assembly name : {module.Assembly.FullName}");
      }
   }

   [Fact]
   public void Projects_Should_HaveDependency_On_SharedKernel()
   {
      var kernelProject = Projects.First(p => p.Type == ProjectType.SharedKernel);
      var projects = Projects.Where(p => p.Type is not ProjectType.SharedKernel and not ProjectType.ModuleIntegration);

      foreach (var project in projects)
      {
         var referencedAssemblies = project.Assembly.GetReferencedAssemblies();
         var hasReference = referencedAssemblies.Any(s => s.Name == kernelProject.AssemblyName);

         Assert.True(hasReference,
            $"module should have dependency on SharedKernel" +
            $"Group name: {project.GroupName}" +
            $"Assembly name : {project.Assembly.FullName}");
      }
   }

   [Fact]
   public void SharedKernel_Should_Not_HaveDependency_On_OtherProjects()
   {
      var sharedKernel = Projects.First(p => p.Type == ProjectType.SharedKernel);
      var otherProjects = Projects
                          .Where(p => p.GroupName != sharedKernel.GroupName)
                          .Select(p => p.AssemblyName)
                          .ToArray();

      var testResult = Types
                       .InAssembly(sharedKernel.Assembly)
                       .ShouldNot()
                       .HaveDependencyOnAny(otherProjects)
                       .GetResult();

      Assert.True(testResult.IsSuccessful, "SharedKernel should not have dependencies on other projects.");
   }

   [Fact]
   public void Modules_Should_Not_Have_Dependency_On_Other_Modules_Except_Integrations()
   {
      foreach (var project in Projects.Where(p => p.Type == ProjectType.Module))
      {
         var forbiddenDependencies = Projects
                                     .Where(p => p.Type == ProjectType.Module && p.GroupName != project.GroupName)
                                     .Select(p => p.AssemblyName)
                                     .ToArray();

         var hasReference = project.Assembly
                                   .GetReferencedAssemblies()
                                   .Select(s => s.Name)
                                   .Intersect(forbiddenDependencies)
                                   .Any();

         Assert.False(hasReference,
            $"module should have dependency on SharedKernel" +
            $"Group name: {project.GroupName}" +
            $"Assembly name : {project.Assembly.FullName}");
      }
   }
}