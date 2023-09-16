// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Attributes.Strategies;

public class ProducesResponseTypeAttributeSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<ProducesResponseTypeAttributeModel>
{
    private readonly ILogger<ProducesResponseTypeAttributeSyntaxGenerationStrategy> logger;

    public ProducesResponseTypeAttributeSyntaxGenerationStrategy(

        ILogger<ProducesResponseTypeAttributeSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, ProducesResponseTypeAttributeModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append('[');

        builder.Append("ProducesResponseType(");

        if (!string.IsNullOrEmpty(model.TypeName))
        {
            builder.Append($"typeof({model.TypeName}), ");
        }

        builder.Append($"(int)HttpStatusCode.{model.HttpStatusCodeName}");

        builder.Append(")]");

        return builder.ToString();
    }
}
