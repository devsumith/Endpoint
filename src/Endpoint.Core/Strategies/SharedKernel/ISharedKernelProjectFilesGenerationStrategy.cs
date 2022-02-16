using Endpoint.Core.Models;

namespace Endpoint.Core.Services
{
    public interface ISharedKernelProjectFilesGenerationStrategy
    {
        void Build(Settings settings);
    }
}
