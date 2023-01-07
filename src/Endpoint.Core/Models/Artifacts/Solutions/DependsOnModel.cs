using Endpoint.Core.Models.Artifacts.Projects;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class DependsOnModel
{
    public ProjectModel Client { get; init; }
    public ProjectModel Service { get; init; }

    public DependsOnModel(ProjectModel client, ProjectModel service)
    {
        Client = client;
        Service = service;
    }
}
