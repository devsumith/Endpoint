// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("react-app-create")]
public class ReactAppCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ReactAppCreateRequestHandler : IRequestHandler<ReactAppCreateRequest>
{
    private readonly ILogger<ReactAppCreateRequestHandler> _logger;
    private readonly IReactService _reactService;
    private readonly ICommandService _commandService;

    public ReactAppCreateRequestHandler(
        ILogger<ReactAppCreateRequestHandler> logger,
        IReactService reactService,
        ICommandService commandService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _reactService = reactService ?? throw new ArgumentNullException(nameof(reactService));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(ReactAppCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ReactAppCreateRequestHandler));

        _reactService.Create(new ReactAppReferenceModel(request.Name, request.Directory));

        _commandService.Start("code .", $"{request.Directory}{Path.DirectorySeparatorChar}{request.Name}");


    }
}
