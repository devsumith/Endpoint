using Endpoint.Core.Models.Options;

namespace Endpoint.Core.Services
{
    public interface IApiProjectFilesGenerationStrategy
    {
        void Build(SettingsModel settings);
        void BuildAdditionalResource(string additionalResource, SettingsModel settings);
        void AddGenerateDocumentationFile(string csProjPath);
    }
}
