// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.SpecFlow.Strategies;

public class SpecFlowHookSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<SpecFlowHookModel>
{
    private readonly ILogger<SpecFlowHookSyntaxGenerationStrategy> _logger;
    public SpecFlowHookSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<SpecFlowHookSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int GetPriority() => 0;

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, SpecFlowHookModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
