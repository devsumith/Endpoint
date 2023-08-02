// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.WebApplications;

public class WebApplicationSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<WebApplicationModel>
{
    private readonly ILogger<WebApplicationSyntaxGenerationStrategy> _logger;
    private readonly ITemplateLocator _templateLocator;
    private readonly ITemplateProcessor _templateProcessor;
    private readonly INamingConventionConverter _namingConventionConverter;
    public WebApplicationSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ITemplateProcessor templateProcessor,
        ITemplateLocator templateLocator,
        ILogger<WebApplicationSyntaxGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
    }

    public override string Create(ISyntaxGenerator syntaxGenerator, WebApplicationModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0}.", model);

        var builder = new StringBuilder();

        var template = _templateLocator.Get("WebApplication");

        builder.AppendLine(_templateProcessor.Process(template, model));

        builder.AppendLine();

        foreach (var entity in model.Entities)
        {
            builder.AppendLine(syntaxGenerator.CreateFor(entity));

            builder.AppendLine();
        }

        builder.AppendLine(syntaxGenerator.CreateFor(new DbContextModel(_namingConventionConverter, model.DbContextName, model.Entities, "")));

        return builder.ToString();
    }
}
