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


[Verb("spec-flow-hook-create")]
public class SpecFlowHookCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SpecFlowHookCreateRequestHandler : IRequestHandler<SpecFlowHookCreateRequest>
{
    private readonly ILogger<SpecFlowHookCreateRequestHandler> _logger;
    private readonly ISpecFlowService _specFlowService;

    public SpecFlowHookCreateRequestHandler(
        ILogger<SpecFlowHookCreateRequestHandler> logger,
        ISpecFlowService specFlowService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _specFlowService = specFlowService ?? throw new ArgumentNullException(nameof(specFlowService));
    }

    public async Task<Unit> Handle(SpecFlowHookCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SpecFlowHookCreateRequestHandler));

        _specFlowService.CreateHook(request.Name, request.Directory);

        return new();
    }
}
