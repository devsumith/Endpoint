using Endpoint.Core.Models.Artifacts.Projects;

namespace Endpoint.Core.Strategies;

public interface IProjectGenerationStrategy
{
    void Create(ProjectModel model);
}
