// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.ExpressionBodies;

public class ExpressionBodyGenerationStrategy : GenericSyntaxGenerationStrategy<ExpressionBodyModel>
{
    private readonly ILogger<ExpressionBodyGenerationStrategy> _logger;

    public ExpressionBodyGenerationStrategy(ILogger<ExpressionBodyGenerationStrategy> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, ExpressionBodyModel model, dynamic? context = null)
    {
        _logger.LogInformation("Generating Expression Body.");

        StringBuilder sb = new StringBuilder();

        sb.AppendLine(model.Body);

        return sb.ToString();
    }
}