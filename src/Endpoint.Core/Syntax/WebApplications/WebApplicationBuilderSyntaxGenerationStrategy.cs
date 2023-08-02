// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.WebApplications;

public class WebApplicationBuilderSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<WebApplicationBuilderModel>
{
    private readonly ILogger<WebApplicationBuilderSyntaxGenerationStrategy> _logger;
    private readonly ITemplateLocator _templateLocator;
    private readonly ITemplateProcessor _templateProcessor;
    public WebApplicationBuilderSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ITemplateLocator templateLocator,
        ITemplateProcessor templateProcessor,
        ILogger<WebApplicationBuilderSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
    }

    public override string Create(ISyntaxGenerator syntaxGenerator, WebApplicationBuilderModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        var template = _templateLocator.Get("WebApplicationBuilder");

        builder.AppendLine(_templateProcessor.Process(template, model));

        return builder.ToString();
    }
}
