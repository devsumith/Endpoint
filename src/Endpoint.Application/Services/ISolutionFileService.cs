using Endpoint.SharedKernal.Models;

namespace Endpoint.Application.Services
{
    public interface ISolutionFileService
    {
        public Settings Build(string name, string dbContextName, bool useShortIdProperty, bool useIntIdPropertyType, string resource, string directory, bool isMicroserviceArchitecture);
    }
}
