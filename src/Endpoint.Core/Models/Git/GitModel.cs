namespace Endpoint.Core.Models.Git;

public class GitModel
{
    public GitModel(string repositoryName)
    {
        RepositoryName = repositoryName;
        Username = Environment.GetEnvironmentVariable("Endpoint:GitUsername");
        Email = Environment.GetEnvironmentVariable("Endpoint:GitEmail");
        PersonalAccessToken = Environment.GetEnvironmentVariable("Endpoint:GitPassword");
        Directory = Environment.CurrentDirectory;
    }

    public string Username { get; init; }
    public string Email { get; init; }
    public string PersonalAccessToken { get; init; }
    public string RepositoryName { get; init; }
    public string Directory { get; init; } 
}
