// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.WebArtifacts.Services;
using Endpoint.Core.Models.WebArtifacts;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Cli.Commands;


[Verb("react-app-create")]
public class ReactAppCreateRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ReactAppCreateRequestHandler : IRequestHandler<ReactAppCreateRequest, Unit>
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

    public async Task<Unit> Handle(ReactAppCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ReactAppCreateRequestHandler));

        _reactService.Create(new ReactAppReferenceModel(request.Name, request.Directory));

        _commandService.Start("code .", $"{request.Directory}{Path.DirectorySeparatorChar}{request.Name}");

        return new();
    }
}
