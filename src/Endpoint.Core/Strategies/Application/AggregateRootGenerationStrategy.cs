using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies.Application;

public class AggregateRootGenerationStrategy : SyntaxGenerationStrategyBase<AggregateRootModel>, IAggregateRootGenerationStrategy
{
    public AggregateRootGenerationStrategy(IServiceProvider serviceProvider)
        :base(serviceProvider)
    { }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, AggregateRootModel model, dynamic configuration = null)
    {
        var content = new List<string>
        {
            $"public class {((Token)model.Name).PascalCase}",

            "{"
        };

        foreach (var property in model.Properties)
        {
            content.Add(($"public {property.Type} {property.Name}" + " { get; set; }").Indent(1));
        }

        content.Add("}");

        return string.Join(Environment.NewLine, content);
    }
}
