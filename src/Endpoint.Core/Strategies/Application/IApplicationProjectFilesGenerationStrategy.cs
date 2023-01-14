using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax.Entities.Legacy;

namespace Endpoint.Core.Services
{
    public interface IApplicationProjectFilesGenerationStrategy
    {
        void Build(SettingsModel settings);
        void BuildAdditionalResource(LegacyAggregateModel aggregateModel, SettingsModel settings);
    }
}
