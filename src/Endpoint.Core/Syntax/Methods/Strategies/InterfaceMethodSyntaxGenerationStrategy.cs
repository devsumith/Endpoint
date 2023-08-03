// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Methods.Strategies;

public class InterfaceMethodSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<MethodModel>
{
    private readonly ILogger<MethodSyntaxGenerationStrategy> _logger;
    public InterfaceMethodSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<MethodSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override bool CanHandle(object model, dynamic context = null)
    {
        return model is MethodModel methodModel && methodModel.Interface;
    }

    public override async Task<string> CreateAsync(ISyntaxGenerator syntaxGenerator, MethodModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        builder.Append($"{await syntaxGenerator.CreateAsync(model.ReturnType)}");

        builder.Append($" {model.Name}");

        builder.Append('(');

        builder.Append(string.Join(',', model.Params.Select(async x => await syntaxGenerator.CreateAsync(x))));

        builder.Append(");");

        return builder.ToString();
    }
}
