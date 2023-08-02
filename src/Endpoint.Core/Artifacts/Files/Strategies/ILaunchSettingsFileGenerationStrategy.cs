
using Endpoint.Core.Options;


namespace Endpoint.Core.Artifacts.Files.Strategies;

public interface ILaunchSettingsFileGenerationStrategy
{
    void Create(SettingsModel settings);
}

