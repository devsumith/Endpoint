using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies
{
    public interface ISettingsFileGenerationStrategy
    {
        bool? CanHandle(Settings model);
        Settings Create(Settings model);
    }
}
