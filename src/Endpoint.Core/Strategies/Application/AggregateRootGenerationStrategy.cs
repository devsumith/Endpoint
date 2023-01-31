using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Entities.Legacy;
using System.Collections.Generic;

namespace Endpoint.Core.Strategies.Application;

public class AggregateRootGenerationStrategy : SyntaxGenerationStrategyBase<LegacyAggregateModel>, IAggregateRootGenerationStrategy
{
    public AggregateRootGenerationStrategy(IServiceProvider serviceProvider)
        :base(serviceProvider)
    { }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, LegacyAggregateModel model, dynamic context = null)
    {
        var content = new List<string>
        {
            $"public class {((SyntaxToken)model.Name).PascalCase}",

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
