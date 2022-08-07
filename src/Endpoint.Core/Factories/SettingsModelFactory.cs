using Endpoint.Core.Models;
using Endpoint.Core.Options;
using System;
using System.IO;

namespace Endpoint.Core.Factories
{
    public static class SettingsModelFactory
    {
        public static SolutionSettingsModel Create(CreateCleanArchitectureMicroserviceOptions options)
        {
            var model = new SolutionSettingsModel
            {
                Directory = $"{options.Directory}{Path.DirectorySeparatorChar}{options.Name}",

            };

            model.Metadata.Add(CoreConstants.SolutionTemplates.CleanArchitectureByJasonTalyor);

            return model;
        }

        public static MetaSolutionSettingsModel Create(string name, string directory)
        {
            return new MetaSolutionSettingsModel
            {
                Directory = directory,
                Name = name
            };
        }

        public static SolutionSettingsModel Resolve(string directory, string name = null)
        {
            throw new NotImplementedException();
        }

        public static MetaSolutionSettingsModel Resolve(string directory)
        {
            throw new NotImplementedException();
        }
    }
}
