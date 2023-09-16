// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.React;
using Endpoint.Core.Artifacts.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<ReactAppCreateRequestHandler> logger;
    private readonly IReactService reactService;
    private readonly ICommandService commandService;

    public ReactAppCreateRequestHandler(
        ILogger<ReactAppCreateRequestHandler> logger,
        IReactService reactService,
        ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.reactService = reactService ?? throw new ArgumentNullException(nameof(reactService));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(ReactAppCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(ReactAppCreateRequestHandler));

        reactService.Create(new ReactAppReferenceModel(request.Name, request.Directory));

        commandService.Start("code .", $"{request.Directory}{Path.DirectorySeparatorChar}{request.Name}");
    }
}
