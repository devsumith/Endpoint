// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Syntax.Methods.Strategies;

public class MethodsSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<List<MethodModel>>
{
    private readonly ILogger<MethodsSyntaxGenerationStrategy> _logger;
    public MethodsSyntaxGenerationStrategy(
        ILogger<MethodsSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, List<MethodModel> model)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        foreach (var method in model)
        {
            builder.AppendLine(await syntaxGenerator.GenerateAsync(method));

            if (method != model.Last())
            {
                builder.AppendLine();
            }
        }

        return builder.ToString();
    }
}

