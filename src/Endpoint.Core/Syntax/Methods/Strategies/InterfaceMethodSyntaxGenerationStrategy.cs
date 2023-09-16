// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using System.Text;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Methods.Strategies;

public class InterfaceMethodSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<MethodModel>
{
    private readonly INamingConventionConverter namingConventionConverter;
    private readonly ILogger<InterfaceMethodSyntaxGenerationStrategy> logger;
    private readonly IContext context;

    public InterfaceMethodSyntaxGenerationStrategy(
        INamingConventionConverter namingConventionConverter,
        ILogger<InterfaceMethodSyntaxGenerationStrategy> logger,
        IContext context)
    {
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public override int GetPriority() => 1;

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, object target)
    {
        if (context.Get<MethodModel>().IsInterface && target is MethodModel)
        {
            return await GenerateAsync(generator, target as MethodModel);
        }

        return null;
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, MethodModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append($"{await syntaxGenerator.GenerateAsync(model.ReturnType)}");

        builder.Append($" {model.Name}");

        builder.Append('(');

        builder.Append(string.Join(',', await Task.WhenAll(model.Params.Select(async x => await syntaxGenerator.GenerateAsync(x)))));

        builder.Append(");");

        return builder.ToString();
    }
}
