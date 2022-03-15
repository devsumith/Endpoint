using Endpoint.Core.Models;
using Endpoint.Core.Strategies;
using System.Collections.Generic;

namespace Endpoint.Core.Generators
{
    public static class AdditionalResourceGenerator
    {
        public static void Generate(Settings model, string resource, List<string> properties, string directory, IAdditionalResourceGenerationStrategyFactory factory)
        {
            factory.CreateFor(model, resource, properties, directory);
        }
    }
}
