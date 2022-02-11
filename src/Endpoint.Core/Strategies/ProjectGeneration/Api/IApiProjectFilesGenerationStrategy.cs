using Endpoint.Core.Models;

namespace Endpoint.Core.Services
{
    public interface IApiProjectFilesGenerationStrategy
    {
        void Build(Settings settings);
        void BuildAdditionalResource(string additionalResource, Settings settings);
        void AddGenerateDocumentationFile(string csProjPath);
    }
}
