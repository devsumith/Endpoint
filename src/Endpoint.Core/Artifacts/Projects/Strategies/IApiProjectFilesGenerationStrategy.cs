
using Endpoint.Core.Options;


namespace Endpoint.Core.Artifacts.Projects.Strategies;

public interface IApiProjectFilesGenerationStrategy
{
    void Build(SettingsModel settings);
    void BuildAdditionalResource(string additionalResource, SettingsModel settings);
    void AddGenerateDocumentationFile(string csProjPath);
}

