using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.Solutions.Crerate
{
    public interface ISolutionGenerationStrategy
    {
        void Create(SolutionModel model);
    }
}
