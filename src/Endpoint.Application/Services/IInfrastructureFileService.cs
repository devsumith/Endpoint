using Endpoint.SharedKernal.Models;

namespace Endpoint.Application.Services
{
    public interface IInfrastructureFileService
    {
        void Build(Settings settings);
        void BuildAdditionalResource(string additionalResource, Settings settings);
    }
}
