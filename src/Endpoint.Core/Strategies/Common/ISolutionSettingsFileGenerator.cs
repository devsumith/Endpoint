
using Endpoint.Core.Options;


namespace Endpoint.Core;

public interface ISolutionSettingsFileGenerator
{
    SolutionSettingsModel CreateFor(SolutionSettingsModel model);
}

