// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.TypeScript.Strategies;

public class TypeScriptTypeSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<TypeScriptTypeModel>
{
    private readonly ILogger<TypeScriptTypeSyntaxGenerationStrategy> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;

    public TypeScriptTypeSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<TypeScriptTypeSyntaxGenerationStrategy> logger,
        INamingConventionConverter namingConventionConverter)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }



    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, TypeScriptTypeModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.AppendLine($"export type {_namingConventionConverter.Convert(NamingConvention.PascalCase, model.Name)}" + " = {");

        foreach (var property in model.Properties)
        {
            builder.AppendLine($"{_namingConventionConverter.Convert(NamingConvention.CamelCase, property.Name)}?: {_namingConventionConverter.Convert(NamingConvention.CamelCase, property.Type.Name)};".Indent(1, 2));
        }

        builder.AppendLine("};");

        return builder.ToString();
    }
}
