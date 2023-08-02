
using Endpoint.Core.Options;


namespace Endpoint.Core.Services;

public interface IInfrastructureProjectFilesGenerationStrategy
{
    void Build(SettingsModel settings);
    void BuildAdditionalResource(string additionalResource, SettingsModel settings);
}

