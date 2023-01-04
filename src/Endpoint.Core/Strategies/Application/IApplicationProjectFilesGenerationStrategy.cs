using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.Services
{
    public interface IApplicationProjectFilesGenerationStrategy
    {
        void Build(SettingsModel settings);
        void BuildAdditionalResource(AggregateRootModel aggregateModel, SettingsModel settings);
    }
}
