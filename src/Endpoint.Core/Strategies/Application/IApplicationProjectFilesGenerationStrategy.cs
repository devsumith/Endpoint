
using Endpoint.Core.Options;
using Endpoint.Core.Syntax.Entities.Legacy;


namespace Endpoint.Core.Services;

public interface IApplicationProjectFilesGenerationStrategy
{
    void Build(SettingsModel settings);
    void BuildAdditionalResource(LegacyAggregatesModel aggregatesModel, SettingsModel settings);
}

