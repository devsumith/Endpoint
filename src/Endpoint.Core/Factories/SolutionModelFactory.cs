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
    }
}
