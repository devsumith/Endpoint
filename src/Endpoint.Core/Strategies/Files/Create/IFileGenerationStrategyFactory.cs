using Endpoint.Core.Models.Files;

namespace Endpoint.Core.Strategies.Files.Create
{
    public interface IFileGenerationStrategyFactory
    {
        void CreateFor(FileModel model);
    }
}
