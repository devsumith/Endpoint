// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Models.Syntax.RequestHandlers;

public class RequestHandlerSyntaxUpdateGenerationStrategy : SyntaxGenerationStrategyBase<RequestHandlerModel>
{
    private readonly ILogger<RequestHandlerSyntaxUpdateGenerationStrategy> _logger;
    private readonly ISyntaxService _syntaxService;

    public RequestHandlerSyntaxUpdateGenerationStrategy(
        IServiceProvider serviceProvider,
        ISyntaxService syntaxService,
        ILogger<RequestHandlerSyntaxUpdateGenerationStrategy> logger)
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
    }

    public override bool CanHandle(object model, dynamic context = null)
        => model is RequestHandlerModel requestHandlerModel && requestHandlerModel.RouteType == RouteType.Update;

    public override string Create(ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory, RequestHandlerModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating syntax for {0} and type {1}.", model);

        var builder = new StringBuilder();


        return builder.ToString();
    }
}
