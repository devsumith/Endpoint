// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.Solutions;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("building-block-create")]
public class BuildingBlockCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; } = "Messaging";


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class BuildingBlockCreateRequestHandler : IRequestHandler<BuildingBlockCreateRequest>
{
    private readonly ILogger<BuildingBlockCreateRequestHandler> _logger;
    private readonly ISolutionService _solutionService;

    public BuildingBlockCreateRequestHandler(
        ILogger<BuildingBlockCreateRequestHandler> logger,
        ISolutionService solutionService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
    }

    public async Task Handle(BuildingBlockCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(BuildingBlockCreateRequestHandler));

        if (request.Name.ToLower() == "messaging")
            _solutionService.MessagingBuildingBlockAdd(request.Directory);

        if (request.Name.ToLower() == "bitpack")
            _solutionService.IOCompressionBuildingBlockAdd(request.Directory);

    }
}
