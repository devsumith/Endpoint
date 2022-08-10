using Endpoint.Core.Models;
using Endpoint.Core.Options;
using System;
using System.IO;
using System.Text.Json;

namespace Endpoint.Core.Factories
{
    public static class SolutionModelFactory
    {
        public static SolutionModel Minimal(CreateEndpointSolutionOptions options)
        {       
            var model = string.IsNullOrEmpty(options.SolutionDirectory) ? new SolutionModel(options.Name, options.Directory) : new SolutionModel(options.Name, options.Directory, options.SolutionDirectory);

            var minimalApiProject = ProjectModelFactory.CreateMinimalApiProject(new CreateMinimalApiProjectOptions
            {
                Name = $"{options.Name}.Api",
                ShortIdPropertyName = false,
                NumericIdPropertyDataType = false,
                Resource = options.Resource,
                Properties = options.Properties,
                Port = 5000,
                Directory = model.SrcDirectory,
                DbContextName = options.DbContextName
            }) ;

            var unitTestProject = ProjectModelFactory.CreateMinimalApiUnitTestsProject(options.Name,model.TestDirectory, options.Resource);

            model.Projects.Add(minimalApiProject);

            model.Projects.Add(unitTestProject);

            model.DependOns.Add(new DependsOnModel(unitTestProject, minimalApiProject));

            return model;
        }

        public static SolutionModel CleanArchitectureMicroservice(CreateCleanArchitectureMicroserviceOptions options)
        {
            var model = string.IsNullOrEmpty(options.SolutionDirectory) ? new SolutionModel(options.Name, options.Directory) : new SolutionModel(options.Name, options.Directory, options.SolutionDirectory);

            var domain = ProjectModelFactory.CreateLibrary($"{options.Name}.Domain", model.SrcDirectory);

            domain.Metadata.Add(CoreConstants.ProjectType.Domain);

            var infrastructure = ProjectModelFactory.CreateLibrary($"{options.Name}.Infrastructure", model.SrcDirectory);

            infrastructure.Metadata.Add(CoreConstants.ProjectType.Infrastructure);

            var application = ProjectModelFactory.CreateLibrary($"{options.Name}.Application", model.SrcDirectory);

            application.Metadata.Add(CoreConstants.ProjectType.Application);

            var api = ProjectModelFactory.CreateWebApi($"{options.Name}.Api", model.SrcDirectory);

            api.Metadata.Add(CoreConstants.ProjectType.Api);

            model.Projects.Add(api);

            model.Projects.Add(domain);

            model.Projects.Add(infrastructure);

            model.Projects.Add(application);

            model.DependOns.Add(new DependsOnModel(infrastructure, domain));

            model.DependOns.Add(new DependsOnModel(application, domain));

            model.DependOns.Add(new DependsOnModel(api, application));

            model.DependOns.Add(new DependsOnModel(api, infrastructure));

            return model;
        }

        public static SolutionModel Workspace(ResolveOrCreateWorkspaceOptions options)
        {
            var model = new SolutionModel(nameof(Workspace), options.Directory, $"{options.Directory}{Path.DirectorySeparatorChar}{options.Name}");

            return model;
        }

        public static SolutionModel Resolve(ResolveOrCreateWorkspaceOptions options)
        {
            var model = new SolutionModel(nameof(Workspace), options.Directory, $"{options.Directory}{Path.DirectorySeparatorChar}{options.Name}");

            return model;
        }

        public static SolutionModel ResolveCleanArchitectureMicroservice(UpdateCleanArchitectureMicroserviceOptions options)
        {
            return new SolutionModel
            {

            };
        }
    }
}
