// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Interfaces;

public class InterfaceSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<InterfaceModel>
{
    private readonly ILogger<InterfaceSyntaxGenerationStrategy> _logger;
    public InterfaceSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<InterfaceSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, InterfaceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append($"public interface {model.Name}");

        if (model.Implements.Count > 0)
        {
            builder.Append(": ");

            builder.Append(string.Join(',', await Task.WhenAll(model.Implements.Select(async x => await syntaxGenerator.GenerateAsync(x, context)))));
        }

        if (model.Properties.Count + model.Methods.Count == 0)
        {
            builder.Append(" { }");

            return builder.ToString();
        }

        builder.AppendLine($"");

        builder.AppendLine("{");

        if (model.Properties.Count > 0)
            builder.AppendLine(((string)await syntaxGenerator.GenerateAsync(model.Properties, context)).Indent(1));

        if (model.Methods.Count > 0)
            builder.AppendLine(((string)await syntaxGenerator.GenerateAsync(model.Methods, this)).Indent(1));

        builder.AppendLine("}");

        return builder.ToString();

    }
}
