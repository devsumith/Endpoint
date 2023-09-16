// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.RequestHandlers;

public class RequestHandlerGetByIdSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<RequestHandlerModel>
{
    private readonly ILogger<RequestHandlerGetByIdSyntaxGenerationStrategy> logger;

    public RequestHandlerGetByIdSyntaxGenerationStrategy(

        ILogger<RequestHandlerGetByIdSyntaxGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int GetPriority() => 0;

    public bool CanHandle(object model)
        => model is RequestHandlerModel requestHandlerModel && requestHandlerModel.RouteType == RouteType.GetById;

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, RequestHandlerModel model)
    {
        logger.LogInformation("Generating syntax for {0} and type {1}.", model);

        var builder = new StringBuilder();

        return builder.ToString();
    }
}
