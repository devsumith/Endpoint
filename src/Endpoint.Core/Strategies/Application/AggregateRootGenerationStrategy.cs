using Endpoint.Core.Models;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies.Application
{
    public interface IAggregateRootGenerationStrategy
    {
        string[] Create(AggregateRootModel model);
    }
    public class AggregateRootGenerationStrategy : IAggregateRootGenerationStrategy
    {
        public string[] Create(AggregateRootModel model)
        {
            var content = new List<string>();

            content.Add($"class {((Token)model.Name).PascalCase}");

            content.Add("{");

            foreach(var property in model.Properties)
            {
                content.Add(($"public {property.Type} {property.Name}" + " { get; set; }").Indent(1));
            }

            content.Add("}");
            
            return content.ToArray();
        }
    }
}
