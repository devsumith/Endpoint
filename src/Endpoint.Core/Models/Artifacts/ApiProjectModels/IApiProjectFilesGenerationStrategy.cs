using Endpoint.Core.Models.Options;

namespace Endpoint.Core.Models.Artifacts.ApiProjectModels
{
    public interface IApiProjectFilesGenerationStrategy
    {
        void Build(SettingsModel settings);
        void BuildAdditionalResource(string additionalResource, SettingsModel settings);
        void AddGenerateDocumentationFile(string csProjPath);
    }
}
