namespace Endpoint.Core.Options;

public class CreateCleanArchitectureMicroserviceOptions
{
    public string Name { get; set; } = string.Empty;
    public string Directory { get; set; } = string.Empty;
    public string SolutionDirectory { get; set; } = string.Empty;

}

public class ResolveOrCreateWorkspaceOptions
{
    public string Directory { get; set; } = string.Empty;
    public string Name { get; set; }
}
