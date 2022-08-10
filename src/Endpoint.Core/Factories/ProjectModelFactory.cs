using Endpoint.Core.Models;
using Endpoint.Core.Options;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public static ProjectModel CreateLibrary(string name, string parentDirectory, List<string>? additionalMetadata = null)
        {
            var project = new ProjectModel(name, parentDirectory);

            if (additionalMetadata != null)
                project.Metadata.Concat(additionalMetadata);

            foreach(var metadataItem in project.Metadata)
            {
                switch (metadataItem) {
                    case CoreConstants.ProjectType.Domain:
                        project.Packages.Add(new PackageModel("FluentValidation", "10.3.6"));
                        project.Packages.Add(new PackageModel("MediatR", "10.0.1"));
                        project.Packages.Add(new PackageModel("Newtonsoft.Json", "13.0.1"));
                        project.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore", "6.0.2"));
                        break;

                    case CoreConstants.ProjectType.Application:
                        project.Packages.Add(new PackageModel("FluentValidation", "10.3.6"));
                        project.Packages.Add(new PackageModel("MediatR", "10.0.1"));
                        project.Packages.Add(new PackageModel("Newtonsoft.Json", "13.0.1"));
                        project.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore", "6.0.2"));
                        break;

                    case CoreConstants.ProjectType.Infrastructure:
                        project.Packages.Add(new PackageModel("FluentValidation", "10.3.6"));
                        project.Packages.Add(new PackageModel("MediatR", "10.0.1"));
                        project.Packages.Add(new PackageModel("Newtonsoft.Json", "13.0.1"));
                        project.Packages.Add(new PackageModel("Microsoft.EntityFrameworkCore", "6.0.2"));
                        break;

                }
            }
            return project;
        }

        public static ProjectModel CreateWebApi(string name, string parentDirectory, List<string>? additionalMetadata = null)
        {
            var project = new ProjectModel(name, parentDirectory)
            {
                DotNetProjectType = DotNetProjectType.WebApi
            };

            if (additionalMetadata != null)
                project.Metadata.Concat(additionalMetadata);

            return project;
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
