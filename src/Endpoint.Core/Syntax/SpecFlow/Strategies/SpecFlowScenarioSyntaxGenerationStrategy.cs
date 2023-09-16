// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.SpecFlow.Strategies;

public class SpecFlowScenarioSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<SpecFlowScenarioModel>
{
    private readonly ILogger<SpecFlowScenarioSyntaxGenerationStrategy> logger;

    public SpecFlowScenarioSyntaxGenerationStrategy(

        ILogger<SpecFlowScenarioSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int GetPriority() => 0;

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, SpecFlowScenarioModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        return builder.ToString();
    }
}
