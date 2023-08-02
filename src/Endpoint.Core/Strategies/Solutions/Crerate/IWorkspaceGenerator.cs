
using Endpoint.Core.Options;


namespace Endpoint.Core.Strategies.Solutions.Crerate;

public interface IWorkspaceGenerator
{
    void CreateFor(WorkspaceSettingsModel model);
}

