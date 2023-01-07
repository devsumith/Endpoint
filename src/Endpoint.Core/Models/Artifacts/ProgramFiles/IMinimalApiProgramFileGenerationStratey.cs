using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.Models.Artifacts.ProgramFiles;

public interface IMinimalApiProgramFileGenerationStratey
{
    void Create(MinimalApiProgramModel model, string directory);
}
