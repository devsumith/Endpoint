using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.Solutions.Update
{
    public interface ISolutionUpdateStrategyFactory
    {
        void UpdateFor(SolutionModel previous, SolutionModel next);
    }
}
