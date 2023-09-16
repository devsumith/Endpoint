// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Expressions;

public class TemplatedExpressionGenerationStrategy : GenericSyntaxGenerationStrategy<TemplateExpressionModel>
{
    private readonly ILogger<TemplatedExpressionGenerationStrategy> logger;
    private readonly ITemplateLocator templateLocator;

    public TemplatedExpressionGenerationStrategy(
        ILogger<TemplatedExpressionGenerationStrategy> logger,
        ITemplateLocator templateLocator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, TemplateExpressionModel model)
    {
        logger.LogInformation("Generating Templated Expression Body");

        StringBuilder sb = new StringBuilder();

        var result = string.Join(Environment.NewLine, templateLocator.Get(model.Template));

        sb.AppendLine(result);

        return sb.ToString();
    }
}