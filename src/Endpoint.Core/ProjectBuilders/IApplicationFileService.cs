using Endpoint.Core.Models;

namespace Endpoint.Core.Services
{
    public interface IApplicationFileService
    {
        void Build(Settings settings);
        void BuildAdditionalResource(string additionalResource, Settings settings);
    }
}
