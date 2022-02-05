using Endpoint.SharedKernal.Models;

namespace Endpoint.Application.Services
{
    public interface IApiFileService
    {
        void Build(Settings settings);
        void BuildAdditionalResource(string additionalResource, Settings settings);
    }
}
