// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.WebApplications;

public class WebApplicationSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<WebApplicationModel>
{
    private readonly ILogger<WebApplicationSyntaxGenerationStrategy> logger;
    private readonly ITemplateLocator templateLocator;
    private readonly ITemplateProcessor templateProcessor;
    private readonly INamingConventionConverter namingConventionConverter;

    public WebApplicationSyntaxGenerationStrategy(
        ITemplateProcessor templateProcessor,
        ITemplateLocator templateLocator,
        ILogger<WebApplicationSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        this.templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, WebApplicationModel model)
    {
        logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        var template = templateLocator.Get("WebApplication");

        builder.AppendLine(templateProcessor.Process(template, model));

        builder.AppendLine();

        foreach (var entity in model.Entities)
        {
            builder.AppendLine(await syntaxGenerator.GenerateAsync(entity));

            builder.AppendLine();
        }

        builder.AppendLine(await syntaxGenerator.GenerateAsync(new DbContextModel(namingConventionConverter, model.DbContextName, model.Entities, string.Empty)));

        return builder.ToString();
    }
}
