using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.Strategies.CSharp
{
    public interface INamespaceGenerationStrategyFactory
    {
        void CreateFor(NamespaceModel model);
    }

    public class NamespaceGenerationStrategyFactory : INamespaceGenerationStrategyFactory
    {
        public void CreateFor(NamespaceModel model)
        {
            throw new System.NotImplementedException();
        }
    }
}
