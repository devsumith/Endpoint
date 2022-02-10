using Endpoint.Core.Models;

namespace Endpoint.Core.Services
{
    public interface IApiFileService
    {
        void Build(Settings settings);
        void BuildAdditionalResource(string additionalResource, Settings settings);
    }
}
