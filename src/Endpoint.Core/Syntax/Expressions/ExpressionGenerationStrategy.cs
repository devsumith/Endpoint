// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Expressions;

public class ExpressionGenerationStrategy : GenericSyntaxGenerationStrategy<ExpressionModel>
{
    private readonly ILogger<ExpressionGenerationStrategy> logger;

    public ExpressionGenerationStrategy(ILogger<ExpressionGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, ExpressionModel model)
    {
        logger.LogInformation("Generating Expression Body.");

        StringBuilder sb = new StringBuilder();

        sb.AppendLine(model.Body);

        return sb.ToString();
    }
}