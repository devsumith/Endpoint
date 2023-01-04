namespace Endpoint.Core.Models;

public class DependsOnModel
{
    public ProjectModel Client { get; init; }
    public ProjectModel Supplier { get; init; }

    public DependsOnModel(ProjectModel client, ProjectModel supplier)
    {
        Client = client;
        Supplier = supplier;
    }
}
