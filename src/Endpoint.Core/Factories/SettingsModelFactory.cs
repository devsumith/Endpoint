using Endpoint.Core.Models;
using Endpoint.Core.Options;
using System.IO;

namespace Endpoint.Core.Factories
{
    public static class SettingsModelFactory
    {
        public static Settings Create(CreateCleanArchitectureMicroserviceOptions options)
        {
            return new Settings
            {
                Directory = $"{options.Directory}{Path.DirectorySeparatorChar}{options.Name}"
            };
        }
    }
}
