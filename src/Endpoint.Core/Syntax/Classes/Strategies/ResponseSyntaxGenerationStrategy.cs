// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.Classes.Strategies;

public class ResponseSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<ResponseAttributeModel>
{
    private readonly ILogger<ResponseSyntaxGenerationStrategy> _logger;
    public ResponseSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<ResponseSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override string Create(ISyntaxGenerator syntaxGenerator, ResponseAttributeModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
