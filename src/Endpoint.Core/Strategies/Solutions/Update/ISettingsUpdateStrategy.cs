using Endpoint.Core.Models.Options;

namespace Endpoint.Core.Strategies.Solutions.Update
{
    public interface ISettingsUpdateStrategy
    {
        bool CanHandle(SolutionSettingsModel model, string entityName, string properties);
        void Update(SolutionSettingsModel model, string entityName, string properties);

        int Order { get; }
    }
}
