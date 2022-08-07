using Endpoint.Core.Models;
using Endpoint.Core.Options;

namespace Endpoint.Core.Factories
{
    public static class SolutionModelFactory
    {
        public static SolutionModel Minimal(CreateEndpointSolutionOptions options)
        {            
            var model = new SolutionModel
            {
                Name = options.Name,
                Directory = options.Directory,
            };

            var minimalApiProject = ProjectModelFactory.CreateMinimalApiProject(new CreateMinimalApiProjectOptions
            {
                Name = options.Name,
                ShortIdPropertyName = options.ShortIdPropertyName,
                NumericIdPropertyDataType = options.NumericIdPropertyDataType,
                Resource = options.Resource,
                Properties = options.Properties,
                Port = options.Port,
                Directory = model.SrcDirectory,
                DbContextName = options.DbContextName
            });

            var unitTestProject = ProjectModelFactory.CreateMinimalApiUnitTestsProject(options.Name,model.TestDirectory, options.Resource);

            model.Projects.Add(minimalApiProject);

            model.Projects.Add(unitTestProject);

            model.DependOns.Add(new DependsOnModel(unitTestProject, minimalApiProject));

            return model;
        }

        public static SolutionModel CleanArchitectureMicroservice(CreateCleanArchitectureMicroserviceOptions options)
        {
            var model = new SolutionModel
            {
                Name = options.Name,
                Directory = options.Directory,
            };

            var domain = ProjectModelFactory.CreateLibrary($"{options.Name}.Domain", model.SrcDirectory);

            var infrastructure = ProjectModelFactory.CreateLibrary($"{options.Name}.Infrastructure", model.SrcDirectory);

            var application = ProjectModelFactory.CreateLibrary($"{options.Name}.Application", model.SrcDirectory);

            var api = ProjectModelFactory.CreateWebApi($"{options.Name}.Api", model.SrcDirectory);

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

        public static SolutionModel ResolveCleanArchitectureMicroservice(UpdateCleanArchitectureMicroserviceOptions options)
        {
            return new SolutionModel
            {

            };
        }
    }
}
