using Endpoint.Core.Models.Artifacts;

namespace Endpoint.Core.Strategies.Solutions.Update
{
    public interface ISolutionUpdateStrategyFactory
    {
        void UpdateFor(SolutionModel previous, SolutionModel next);
    }
}
