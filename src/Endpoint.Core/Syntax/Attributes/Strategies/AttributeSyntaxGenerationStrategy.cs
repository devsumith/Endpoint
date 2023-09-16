// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Attributes.Strategies;

public class AttributeSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<AttributeModel>
{
    private readonly ILogger<AttributeSyntaxGenerationStrategy> logger;

    public AttributeSyntaxGenerationStrategy(

        ILogger<AttributeSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, AttributeModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append('[');

        builder.Append(model.Name);

        if (model.Template != null && model.Properties.Count == 0)
        {
            builder.Append($"({model.Template})");
        }

        if (model.Properties.Count == 1)
        {
            builder.Append('(');

            if (model.Template != null)
            {
                builder.Append($"{model.Template}, ");
            }

            foreach (var property in model.Properties)
            {
                builder.Append($"{property.Key} = \"{property.Value}\"");
            }

            builder.Append(')');
        }

        if (model.Properties.Count > 1)
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
