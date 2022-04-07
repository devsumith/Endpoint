using Endpoint.Core.Options;
using Endpoint.Core.Strategies;

namespace Endpoint.Core.Generators
{
    public static class AdditionalResourceGenerator
    {
        public static void Generate(AddResourceOptions options, IAdditionalResourceGenerationStrategyFactory factory)
        {
            factory.CreateFor(options);
        }
    }
}
