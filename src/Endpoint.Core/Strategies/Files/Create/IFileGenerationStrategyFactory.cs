
using Endpoint.Core.Artifacts.Files;


namespace Endpoint.Core.Strategies.Files.Create;

public interface IFileGenerator
{
    void CreateFor(FileModel model);
}

