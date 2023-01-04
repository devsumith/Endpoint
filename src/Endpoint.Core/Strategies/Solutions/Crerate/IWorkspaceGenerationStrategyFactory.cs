using Endpoint.Core.Models.Options;

namespace Endpoint.Core.Strategies.Solutions.Crerate
{
    public interface IWorkspaceGenerationStrategyFactory
    {
        void CreateFor(WorkspaceSettingsModel model);
    }

}
