// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.SpecFlow.Strategies;

public class SpecFlowFeatureSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<SpecFlowFeatureModel>
{
    private readonly ILogger<SpecFlowFeatureSyntaxGenerationStrategy> logger;

    public SpecFlowFeatureSyntaxGenerationStrategy(

        ILogger<SpecFlowFeatureSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int GetPriority() => 0;

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, SpecFlowFeatureModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        return builder.ToString();
    }
}
