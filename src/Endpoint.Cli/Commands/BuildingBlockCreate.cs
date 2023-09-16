// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Solutions.Services;
using MediatR;
using Microsoft.Extensions.Logging;

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
    private readonly ILogger<BuildingBlockCreateRequestHandler> logger;
    private readonly ISolutionService solutionService;

    public BuildingBlockCreateRequestHandler(
        ILogger<BuildingBlockCreateRequestHandler> logger,
        ISolutionService solutionService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
    }

    public async Task Handle(BuildingBlockCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(BuildingBlockCreateRequestHandler));

        if (request.Name.ToLower() == "messaging")
        {
            await solutionService.MessagingBuildingBlockAdd(request.Directory);
        }

        if (request.Name.ToLower() == "bitpack")
        {
            await solutionService.IOCompressionBuildingBlockAdd(request.Directory);
        }
    }
}
