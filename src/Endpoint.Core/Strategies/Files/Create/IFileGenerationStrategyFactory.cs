using Endpoint.Core.Models.Artifacts.Files;

namespace Endpoint.Core.Strategies.Files.Create
{
    public interface IFileGenerationStrategyFactory
    {
        void CreateFor(FileModel model);
    }
}
