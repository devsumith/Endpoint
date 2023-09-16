// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Text;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.TypeScript.Strategies;

public class FunctionSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<FunctionModel>
{
    private readonly ILogger<FunctionSyntaxGenerationStrategy> logger;
    private readonly INamingConventionConverter namingConventionConverter;

    public FunctionSyntaxGenerationStrategy(

        INamingConventionConverter namingConventionConverter,
        ILogger<FunctionSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, FunctionModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        foreach (var import in model.Imports)
        {
            builder.AppendLine(await syntaxGenerator.GenerateAsync(import));

            if (import == model.Imports.Last())
            {
                builder.AppendLine();
            }
        }

        builder.AppendLine($"export function {namingConventionConverter.Convert(NamingConvention.CamelCase, model.Name)}()" + " {");

        builder.AppendLine(model.Body.Indent(1, 2));

        builder.AppendLine("};");

        return builder.ToString();
    }
}
