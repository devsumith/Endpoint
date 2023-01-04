using Endpoint.Core.Models.Artifacts;

namespace Endpoint.Core.Strategies;

public interface IProjectGenerationStrategy
{
    void Create(ProjectModel model);
}
