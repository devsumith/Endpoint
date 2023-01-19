using Endpoint.Core.Models.Options;

namespace Endpoint.Core.Models.Artifacts.Files.Strategies
{
    public interface ILaunchSettingsFileGenerationStrategy
    {
        void Create(SettingsModel settings);
    }
}
