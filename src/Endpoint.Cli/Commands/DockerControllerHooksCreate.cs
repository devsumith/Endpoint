// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.Artifacts.SpecFlow;

namespace Endpoint.Cli.Commands;


[Verb("docker-controller-hooks-create")]
public class DockerControllerHooksCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class DockerControllerHooksCreateRequestHandler : IRequestHandler<DockerControllerHooksCreateRequest>
{
    private readonly ILogger<DockerControllerHooksCreateRequestHandler> _logger;
    private readonly ISpecFlowService _specFlowService;

    public DockerControllerHooksCreateRequestHandler(
        ILogger<DockerControllerHooksCreateRequestHandler> logger,
        ISpecFlowService specFlowService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _specFlowService = specFlowService ?? throw new ArgumentNullException(nameof(specFlowService));
    }

    public async Task Handle(DockerControllerHooksCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(DockerControllerHooksCreateRequestHandler));

        _specFlowService.CreateDockerControllerHooks(request.Directory);


    }
}
