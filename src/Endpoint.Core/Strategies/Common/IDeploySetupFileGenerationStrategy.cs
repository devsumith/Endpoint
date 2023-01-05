using Endpoint.Core.Models.Options;

namespace Endpoint.Core.Strategies.Common;

public interface IDeploySetupFileGenerationStrategy
{
    void Generate(SettingsModel settings);
}
