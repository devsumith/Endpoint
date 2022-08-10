using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.WorkspaceSettingss.Update
{
    public interface IWorkspaceSettingsUpdateStrategyFactory
    {
        void UpdateFor(WorkspaceSettingsModel previous, WorkspaceSettingsModel next);
    }
}
