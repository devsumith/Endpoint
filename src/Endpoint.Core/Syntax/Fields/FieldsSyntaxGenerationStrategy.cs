// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Fields;

public class FieldsSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<List<FieldModel>>
{
    private readonly ILogger<FieldsSyntaxGenerationStrategy> logger;

    public FieldsSyntaxGenerationStrategy(

        ILogger<FieldsSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, List<FieldModel> model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        foreach (var field in model)
        {
            builder.AppendLine(await CreateAsync(syntaxGenerator, field));

            if (field != model.Last())
            {
                builder.AppendLine();
            }
        }

        return builder.ToString();
    }

    private async Task<string> CreateAsync(ISyntaxGenerator syntaxGenerator, FieldModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append(await syntaxGenerator.GenerateAsync(model.AccessModifier));

        if (model.ReadOnly)
        {
            builder.Append(" readonly");
        }

        if (!string.IsNullOrEmpty(model.DefaultValue))
        {
            builder.Append($" {await syntaxGenerator.GenerateAsync(model.Type)} {model.Name} = {model.DefaultValue};");
        }
        else
        {
            builder.Append($" {await syntaxGenerator.GenerateAsync(model.Type)} {model.Name};");
        }

        return builder.ToString();
    }
}
