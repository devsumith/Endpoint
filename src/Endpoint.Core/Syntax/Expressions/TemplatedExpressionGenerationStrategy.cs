// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.Expressions;

public class TemplatedExpressionGenerationStrategy : GenericSyntaxGenerationStrategy<TemplateExpressionModel>
{
    private readonly ILogger<TemplatedExpressionGenerationStrategy> _logger;
    private readonly ITemplateLocator _templateLocator;

    public TemplatedExpressionGenerationStrategy(
        ILogger<TemplatedExpressionGenerationStrategy> logger,
        ITemplateLocator templateLocator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator generator, TemplateExpressionModel model)
    {
        _logger.LogInformation("Generating Templated Expression Body");

        StringBuilder sb = new StringBuilder();

        var result = string.Join(Environment.NewLine, _templateLocator.Get(model.Template));

        sb.AppendLine(result);

        return sb.ToString();
    }
}