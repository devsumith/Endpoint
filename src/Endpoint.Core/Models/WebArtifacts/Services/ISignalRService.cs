namespace Endpoint.Core.Models.WebArtifacts.Services;

public interface ISignalRService
{
    void Add(string directory);

    void AddHub(string name, string directory);

}

