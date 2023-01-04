using Endpoint.Core.Models.Artifacts;

namespace Endpoint.Core.Strategies.Files.Create
{
    public interface IFileGenerationStrategyFactory
    {
        void CreateFor(FileModel model);
    }
}
