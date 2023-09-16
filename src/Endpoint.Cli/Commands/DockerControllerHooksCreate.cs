// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.SpecFlow;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<DockerControllerHooksCreateRequestHandler> logger;
    private readonly ISpecFlowService specFlowService;

    public DockerControllerHooksCreateRequestHandler(
        ILogger<DockerControllerHooksCreateRequestHandler> logger,
        ISpecFlowService specFlowService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.specFlowService = specFlowService ?? throw new ArgumentNullException(nameof(specFlowService));
    }

    public async Task Handle(DockerControllerHooksCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(DockerControllerHooksCreateRequestHandler));

        specFlowService.CreateDockerControllerHooks(request.Directory);
    }
}
