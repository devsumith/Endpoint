// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.SpecFlow.Strategies;

public class SpecFlowStepsSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<SpecFlowStepsModel>
{
    private readonly ILogger<SpecFlowStepsSyntaxGenerationStrategy> _logger;
    public SpecFlowStepsSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<SpecFlowStepsSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, SpecFlowStepsModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
