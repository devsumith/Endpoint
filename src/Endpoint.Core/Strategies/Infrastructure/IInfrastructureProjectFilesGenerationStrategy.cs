using Endpoint.Core.Models;

namespace Endpoint.Core.Services
{
    public interface IInfrastructureProjectFilesGenerationStrategy
    {
        void Build(Settings settings);
        void BuildAdditionalResource(string additionalResource, Settings settings);
    }
}
