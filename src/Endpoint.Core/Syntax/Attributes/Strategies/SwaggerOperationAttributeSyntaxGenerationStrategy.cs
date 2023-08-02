// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.Attributes.Strategies;

public class SwaggerOperationAttributeSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<SwaggerOperationAttributeModel>
{
    private readonly ILogger<SwaggerOperationAttributeSyntaxGenerationStrategy> _logger;
    public SwaggerOperationAttributeSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<SwaggerOperationAttributeSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerator syntaxGenerator, SwaggerOperationAttributeModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.AppendLine("[SwaggerOperation(");

        builder.AppendLine($"Summary = \"{model.Summary}\",".Indent(1));

        builder.AppendLine($"Description = @\"{model.Description}\"".Indent(1));

        builder.Append(")]");

        return builder.ToString();
    }
}
