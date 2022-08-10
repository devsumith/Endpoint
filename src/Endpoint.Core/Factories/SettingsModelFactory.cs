using Endpoint.Core.Models;
using Endpoint.Core.Options;
using System;
using System.IO;

namespace Endpoint.Core.Factories
{
    public static class SolutionSettingsModelFactory
    {
        public static SolutionSettingsModel Create(CreateCleanArchitectureMicroserviceOptions options)
        {
            var model = new SolutionSettingsModel
            {
                Directory = $"{options.Directory}{Path.DirectorySeparatorChar}{options.Name}",
                Namespace = options.Name
            };

            model.Metadata.Add(CoreConstants.SolutionTemplates.CleanArchitectureByJasonTalyor);

            return model;
        }

        public static SolutionSettingsModel Resolve(string directory, string name = null)
        {
            throw new NotImplementedException();
        }

        public static WorkspaceSettingsModel Resolve(string directory)
        {
            return new WorkspaceSettingsModel
            {

            };
        }
    }
}
