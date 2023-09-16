// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax;

public class RuleForSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<RuleForModel>
{
    private readonly ILogger<RuleForSyntaxGenerationStrategy> logger;

    public RuleForSyntaxGenerationStrategy(

        ILogger<RuleForSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int GetPriority() => 0;

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, RuleForModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        return builder.ToString();
    }
}
