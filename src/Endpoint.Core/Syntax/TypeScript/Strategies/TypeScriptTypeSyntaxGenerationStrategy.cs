// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.TypeScript.Strategies;

public class TypeScriptTypeSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<TypeScriptTypeModel>
{
    private readonly ILogger<TypeScriptTypeSyntaxGenerationStrategy> logger;
    private readonly INamingConventionConverter namingConventionConverter;

    public TypeScriptTypeSyntaxGenerationStrategy(

        ILogger<TypeScriptTypeSyntaxGenerationStrategy> logger,
        INamingConventionConverter namingConventionConverter)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, TypeScriptTypeModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.AppendLine($"export type {namingConventionConverter.Convert(NamingConvention.PascalCase, model.Name)}" + " = {");

        foreach (var property in model.Properties)
        {
            builder.AppendLine($"{namingConventionConverter.Convert(NamingConvention.CamelCase, property.Name)}?: {namingConventionConverter.Convert(NamingConvention.CamelCase, property.Type.Name)};".Indent(1, 2));
        }

        builder.AppendLine("};");

        return builder.ToString();
    }
}
