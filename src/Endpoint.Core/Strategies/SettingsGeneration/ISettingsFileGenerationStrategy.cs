using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies
{
    public interface ISettingsFileGenerationStrategy
    {
        Settings Create();
    }
}
