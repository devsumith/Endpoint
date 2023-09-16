// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Attributes.Strategies;

public class SwaggerOperationAttributeSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<SwaggerOperationAttributeModel>
{
    private readonly ILogger<SwaggerOperationAttributeSyntaxGenerationStrategy> logger;

    public SwaggerOperationAttributeSyntaxGenerationStrategy(

        ILogger<SwaggerOperationAttributeSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, SwaggerOperationAttributeModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.AppendLine("[SwaggerOperation(");

        builder.AppendLine($"Summary = \"{model.Summary}\",".Indent(1));

        builder.AppendLine($"Description = @\"{model.Description}\"".Indent(1));

        builder.Append(")]");

        return builder.ToString();
    }
}
