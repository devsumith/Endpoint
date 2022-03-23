using Endpoint.Core.Models;

namespace Endpoint.Core.Strategies
{
    public interface IFileGenerationStrategy
    {
        bool CanHandle(FileModel model);

        void Create(dynamic model);
        int Order { get; }
    }
}
