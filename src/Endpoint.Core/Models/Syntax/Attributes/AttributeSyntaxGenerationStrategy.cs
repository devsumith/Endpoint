using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Attributes;

public class AttributeSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<AttributeModel>
{
    private readonly ILogger<AttributeSyntaxGenerationStrategy> _logger;
    public AttributeSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<AttributeSyntaxGenerationStrategy> logger) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, AttributeModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append('[');

        builder.Append(model.Name);

        if(model.Properties.Count == 1)
        {
            builder.Append('(');

            foreach (var property in model.Properties)
            {
                builder.Append($"{property.Key} = \"{property.Value}\"");
            }

            builder.Append(')');
        }

        if(model.Properties.Count > 1)
        {
            builder.AppendLine("(");

            foreach (var property in model.Properties)
            {
                var propertyKeyValuePair = new StringBuilder($"{property.Key} = \"{property.Value}\"");

                if (property.Key != model.Properties.Last().Key)
                {
                    propertyKeyValuePair.Append(',');
                }

                builder.AppendLine($"{propertyKeyValuePair}".Indent(1));
            }

            builder.Append(')');
        }

        builder.Append(']');


        return builder.ToString();
    }
}