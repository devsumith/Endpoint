using Endpoint.Core.Models;

namespace Endpoint.Core.Services
{
    public interface ISharedKernalProjectFilesGenerationStrategy
    {
        void Build(Settings settings);
    }
}
