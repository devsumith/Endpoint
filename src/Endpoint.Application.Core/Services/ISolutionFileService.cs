using Endpoint.SharedKernal.Models;
using System.Collections.Generic;

namespace Endpoint.Application.Services
{
    public interface ISolutionFileService
    {
        public Settings Build(string name, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, string resource, string directory, bool isMicroserviceArchitecture);

        public Settings Build(string name, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, List<string> resources, string directory, bool isMicroserviceArchitecture);
    }
}
