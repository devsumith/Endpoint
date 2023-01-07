using Endpoint.Core.Models.Options;

namespace Endpoint.Core.Models.Artifacts.Files
{
    public interface ILaunchSettingsFileGenerationStrategy
    {
        void Create(SettingsModel settings);
    }
}
