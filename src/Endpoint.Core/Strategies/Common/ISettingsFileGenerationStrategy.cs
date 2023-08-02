
using Endpoint.Core.Options;


namespace Endpoint.Core.Strategies;

public interface ISolutionSettingsFileGenerationStrategy
{
    bool? CanHandle(SolutionSettingsModel model);
    SolutionSettingsModel Create(SolutionSettingsModel model);
}

