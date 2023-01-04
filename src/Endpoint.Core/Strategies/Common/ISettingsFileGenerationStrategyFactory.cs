using Endpoint.Core.Models.Options;

namespace Endpoint.Core
{
    public interface ISolutionSettingsFileGenerationStrategyFactory
    {
        SolutionSettingsModel CreateFor(SolutionSettingsModel model);
    }
}
