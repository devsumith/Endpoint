// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Syntax.RequestHandlers;

public class RequestHandlerSyntaxUpdateGenerationStrategy : GenericSyntaxGenerationStrategy<RequestHandlerModel>
{
    private readonly ILogger<RequestHandlerSyntaxUpdateGenerationStrategy> logger;
    private readonly ISyntaxService syntaxService;

    public RequestHandlerSyntaxUpdateGenerationStrategy(

        ISyntaxService syntaxService,
        ILogger<RequestHandlerSyntaxUpdateGenerationStrategy> logger)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
    }

    public int GetPriority() => 0;

    public bool CanHandle(object model)
        => model is RequestHandlerModel requestHandlerModel && requestHandlerModel.RouteType == RouteType.Update;

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, RequestHandlerModel model)
    {
        logger.LogInformation("Generating syntax for {0} and type {1}.", model);

        var builder = new StringBuilder();

        return builder.ToString();
    }
}
