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

[Verb("spec-flow-hook-create")]
public class SpecFlowHookCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SpecFlowHookCreateRequestHandler : IRequestHandler<SpecFlowHookCreateRequest>
{
    private readonly ILogger<SpecFlowHookCreateRequestHandler> logger;
    private readonly ISpecFlowService specFlowService;

    public SpecFlowHookCreateRequestHandler(
        ILogger<SpecFlowHookCreateRequestHandler> logger,
        ISpecFlowService specFlowService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.specFlowService = specFlowService ?? throw new ArgumentNullException(nameof(specFlowService));
    }

    public async Task Handle(SpecFlowHookCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(SpecFlowHookCreateRequestHandler));

        specFlowService.CreateHook(request.Name, request.Directory);
    }
}
