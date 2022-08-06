using Endpoint.Core.Models;

namespace Endpoint.Core
{
    public interface ISettingsFileGenerationStrategyFactory
    {
        Settings CreateFor(Settings model);
    }
}
