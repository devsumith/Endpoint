using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies.Solutions.Update
{
    public interface ISettingsUpdateStrategyFactory
    {
        public void Update(SolutionSettingsModel model, string entityName, string properties);
    }
}
