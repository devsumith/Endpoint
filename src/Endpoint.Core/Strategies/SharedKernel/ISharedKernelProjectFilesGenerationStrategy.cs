
using Endpoint.Core.Options;


namespace Endpoint.Core.Services;

public interface ISharedKernelProjectFilesGenerationStrategy
{
    void Build(SettingsModel settings);
}

