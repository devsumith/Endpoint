using Endpoint.Core.Models.Options;

namespace Endpoint.Core.Strategies
{
    public class MinimalApiSettingsFileGenerationStrategy : ISolutionSettingsFileGenerationStrategy
    {
        public bool? CanHandle(SolutionSettingsModel request) => request.Metadata.Contains(CoreConstants.SolutionTemplates.Minimal);

        public SolutionSettingsModel Create(SolutionSettingsModel request)
        {

            return new();
        }
    }
}
