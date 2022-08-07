using Endpoint.Core.Models;

namespace Endpoint.Core
{
    public interface ISolutionSettingsFileGenerationStrategyFactory
    {
        SolutionSettingsModel CreateFor(SolutionSettingsModel model);
    }
}
