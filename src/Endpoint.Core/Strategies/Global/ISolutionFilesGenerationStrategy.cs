using Endpoint.Core.Models;
using System.Collections.Generic;

namespace Endpoint.Core.Services
{
    public interface ISolutionFilesGenerationStrategy
    {
        public Settings Build(string name, string properties, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, string resource, string directory, bool isMicroserviceArchitecture, List<string> plugins, string prefix);

        public Settings Build(string name, string properties, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, List<string> resources, string directory, bool isMicroserviceArchitecture, List<string> plugins, string prefix);
    }
}
