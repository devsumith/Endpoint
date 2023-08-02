// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.TypeScript.Strategies;

public class FunctionSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<FunctionModel>
{
    private readonly ILogger<FunctionSyntaxGenerationStrategy> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;

    public FunctionSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        INamingConventionConverter namingConventionConverter,
        ILogger<FunctionSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public override string Create(ISyntaxGenerator syntaxGenerator, FunctionModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        foreach (var import in model.Imports)
        {
            builder.AppendLine(syntaxGenerator.CreateFor(import));

            if (import == model.Imports.Last())
            {
                builder.AppendLine();
            }
        }

        builder.AppendLine($"export function {_namingConventionConverter.Convert(NamingConvention.CamelCase, model.Name)}()" + " {");

        builder.AppendLine(model.Body.Indent(1, 2));

        builder.AppendLine("};");

        return builder.ToString();
    }
}
