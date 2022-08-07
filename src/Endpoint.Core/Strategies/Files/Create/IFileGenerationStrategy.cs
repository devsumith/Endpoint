using Endpoint.Core.Models.Files;

namespace Endpoint.Core.Strategies.Files.Create
{
    public interface IFileGenerationStrategy
    {
        bool CanHandle(FileModel model);
        void Create(dynamic model);
        int Order { get; }
    }
}
