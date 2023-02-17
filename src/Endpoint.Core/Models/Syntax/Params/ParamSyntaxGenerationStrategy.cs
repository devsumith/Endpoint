// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.Params;

public class ParamSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<ParamModel>
{
    private readonly ILogger<ParamSyntaxGenerationStrategy> _logger;
    public ParamSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<ParamSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, ParamModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        if (model.ExtensionMethodParam)
            builder.Append("this ");

        if (model.Attribute != null)
            builder.Append(syntaxGenerationStrategyFactory.CreateFor(model.Attribute));

        builder.Append($"{syntaxGenerationStrategyFactory.CreateFor(model.Type)} {model.Name}");

        return builder.ToString();
    }
}
