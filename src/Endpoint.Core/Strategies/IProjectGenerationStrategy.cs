using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies;

public interface IProjectGenerationStrategy
{
    void Create(ProjectModel model);
}
