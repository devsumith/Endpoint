using Endpoint.Core.Models;
using Endpoint.Core.Options;
using System;

namespace Endpoint.Core.Factories
{
    public static class ProjectModelFactory
    {
        public static ProjectModel CreateMinimalApiProject(CreateMinimalApiProjectOptions options)
        {
            var projectModel = new ProjectModel(DotNetProjectType.MinimalWebApi, options.Name, options.Directory);

            projectModel.GenerateDocumentationFile = true;

            projectModel.Files.Add(FileModelFactory.LaunchSettingsJson(projectModel.Directory,projectModel.Name, options.Port.Value));

            projectModel.Files.Add(FileModelFactory.AppSettings(projectModel.Directory, projectModel.Name, options.DbContextName));

            projectModel.Files.Add(FileModelFactory.MinimalApiProgram(projectModel.Directory, options.Resource,options.Properties,options.ShortIdPropertyName.Value,options.NumericIdPropertyDataType.Value,options.DbContextName));

            projectModel.Packages.Add(new() { Name = "Microsoft.EntityFrameworkCore.InMemory", Version = "6.0.2" });

            projectModel.Packages.Add(new() { Name = "Swashbuckle.AspNetCore.Annotations", Version = "6.2.3" });

            projectModel.Packages.Add(new() { Name = "Swashbuckle.AspNetCore.Newtonsoft", Version = "6.2.3" });

            projectModel.Packages.Add(new() { Name = "MinimalApis.Extensions", IsPreRelease = true });

            return projectModel;
        }

        public static ProjectModel CreateMinimalApiUnitTestsProject(string name, string directory, string resource)
        {
            var model = new ProjectModel(DotNetProjectType.XUnit, $"{name}.Tests", directory);

            return model;
        }

        public static ProjectModel CreateTestingProject()
        {
            throw new NotImplementedException();
        }

        public static ProjectModel CreateUnitTestsProject()
        {
            throw new NotImplementedException();
        }

        public static ProjectModel CreateIntegrationTestsProject()
        {
            throw new NotImplementedException();
        }
    }
}
