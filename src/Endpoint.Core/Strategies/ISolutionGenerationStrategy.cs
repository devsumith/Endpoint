using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies
{
    public interface ISolutionGenerationStrategy
    {
        void Create(SolutionModel model);
    }
}
