// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Methods.Strategies;

public class MethodSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<MethodModel>
{
    private readonly ILogger<MethodSyntaxGenerationStrategy> _logger;
    public MethodSyntaxGenerationStrategy(
        ILogger<MethodSyntaxGenerationStrategy> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, MethodModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        foreach (var attribute in model.Attributes)
        {
            builder.AppendLine(await syntaxGenerator.GenerateAsync(attribute));
        }

        builder.Append(await syntaxGenerator.GenerateAsync(model.AccessModifier));

        if (model.Override)
            builder.Append(" override");

        if (model.Async)
            builder.Append(" async");

        if (model.Params.SingleOrDefault(x => x.ExtensionMethodParam) != null || model.Static)
            builder.Append(" static");

        builder.Append($" {await syntaxGenerator.GenerateAsync(model.ReturnType)}");

        builder.Append($" {model.Name}");

        builder.Append('(');

        builder.Append(string.Join(',', await Task.WhenAll(model.Params.Select(async x => await syntaxGenerator.GenerateAsync(x)))));

        builder.Append(')');

        if (model.Body == null)
            builder.Append("{ }");
        else
        {
            builder.AppendLine();

            builder.AppendLine("{");

            string body = await syntaxGenerator.GenerateAsync(model.Body);

            builder.AppendLine(body.Indent(1));

            builder.Append('}');
        }

        return builder.ToString();
    }
}

