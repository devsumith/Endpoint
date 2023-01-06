using Endpoint.Core.Models.Artifacts.Solutions;

namespace Endpoint.Core.Strategies.Solutions.Update
{
    public interface ISolutionUpdateStrategy
    {
        bool CanHandle(SolutionModel previous, SolutionModel next);
        void Update(SolutionModel previous, SolutionModel next);
        int Order { get; set; }
    }
}
