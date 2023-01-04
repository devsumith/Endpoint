using Endpoint.Core.Models.Artifacts;

namespace Endpoint.Core.Strategies.Solutions.Crerate
{
    public interface ISolutionGenerationStrategy
    {
        void Create(SolutionModel model);
    }
}
