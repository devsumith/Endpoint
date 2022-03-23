using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies
{
    public interface IFileGenerationStrategyFactory
    {
        void CreateFor(FileModel model);
    }
}
