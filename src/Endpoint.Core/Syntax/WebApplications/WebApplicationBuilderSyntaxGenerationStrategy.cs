// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.WebApplications;

public class WebApplicationBuilderSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<WebApplicationBuilderModel>
{
    private readonly ILogger<WebApplicationBuilderSyntaxGenerationStrategy> logger;
    private readonly ITemplateLocator templateLocator;
    private readonly ITemplateProcessor templateProcessor;

    public WebApplicationBuilderSyntaxGenerationStrategy(

        ITemplateLocator templateLocator,
        ITemplateProcessor templateProcessor,
        ILogger<WebApplicationBuilderSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
        this.templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, WebApplicationBuilderModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        var template = templateLocator.Get("WebApplicationBuilder");

        builder.AppendLine(templateProcessor.Process(template, model));

        return builder.ToString();
    }
}
