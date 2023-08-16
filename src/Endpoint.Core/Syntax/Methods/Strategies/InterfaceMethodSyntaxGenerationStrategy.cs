// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Interfaces;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Methods.Strategies;

public class InterfaceMethodSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<MethodModel>
{
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ILogger<InterfaceMethodSyntaxGenerationStrategy> _logger;
    public InterfaceMethodSyntaxGenerationStrategy(
        INamingConventionConverter namingConventionConverter,
        ILogger<InterfaceMethodSyntaxGenerationStrategy> logger)
    {
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override int GetPriority() => 1;

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, object target, dynamic context = null)
    {
        if (context != null && context is InterfaceSyntaxGenerationStrategy && target is MethodModel)
        {
            return await GenerateAsync(generator, target as MethodModel, context);
        }

        return null;
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, MethodModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append($"{await syntaxGenerator.GenerateAsync(model.ReturnType)}");

        builder.Append($" {model.Name}");

        builder.Append('(');

        builder.Append(string.Join(',', await Task.WhenAll(model.Params.Select(async x => await syntaxGenerator.GenerateAsync(x)))));

        builder.Append(");");

        return builder.ToString();
    }
}
