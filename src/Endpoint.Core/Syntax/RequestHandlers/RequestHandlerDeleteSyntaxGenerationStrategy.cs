// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Syntax.RequestHandlers;

public class RequestHandlerDeleteSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<RequestHandlerModel>
{
    private readonly ILogger<RequestHandlerDeleteSyntaxGenerationStrategy> _logger;
    public RequestHandlerDeleteSyntaxGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<RequestHandlerDeleteSyntaxGenerationStrategy> logger)

    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public int GetPriority() => 0;


    public bool CanHandle(object model)
        => model is RequestHandlerModel requestHandlerModel && requestHandlerModel.RouteType == RouteType.Delete;
    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, RequestHandlerModel model)
    {
        _logger.LogInformation("Generating syntax for {0} and type {1}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
