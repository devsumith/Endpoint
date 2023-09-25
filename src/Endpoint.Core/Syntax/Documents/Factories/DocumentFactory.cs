// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.Documents.Factories;

public class DocumentFactory : IDocumentFactory
{
    private readonly ILogger<DocumentFactory> logger;

    public DocumentFactory(ILogger<DocumentFactory> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<DocumentModel> CreateCommandAsync(ClassModel aggregate, RouteType routeType)
    {
        logger.LogInformation("Creating Command Document. {aggregateName}", aggregate.Name);

        var model = new DocumentModel();

        model.Name = routeType switch
        {
            RouteType.Create => $"Create{aggregate.Name}",
            RouteType.Delete => $"Delete{aggregate.Name}",
            RouteType.Update => $"Update{aggregate.Name}",
            _ => throw new Exception()
        };
        ;
        return model;
    }
}
