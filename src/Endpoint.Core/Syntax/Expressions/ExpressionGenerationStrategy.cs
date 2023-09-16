// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.Expressions;

public class ExpressionGenerationStrategy : GenericSyntaxGenerationStrategy<ExpressionModel>
{
    private readonly ILogger<ExpressionGenerationStrategy> _logger;

    public ExpressionGenerationStrategy(ILogger<ExpressionGenerationStrategy> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, ExpressionModel model)
    {
        _logger.LogInformation("Generating Expression Body.");

        StringBuilder sb = new StringBuilder();

        sb.AppendLine(model.Body);

        return sb.ToString();
    }
}