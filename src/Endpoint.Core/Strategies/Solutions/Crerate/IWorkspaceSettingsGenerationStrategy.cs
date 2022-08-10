using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.Solutions.Crerate
{
    public interface IWorkspaceSettingsGenerationStrategy
    {
        int Order { get; set; }
        bool CanHandle(WorkspaceSettingsModel model);
        void Create(WorkspaceSettingsModel model);
    }

}
