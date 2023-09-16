// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Solutions.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("solution-create-from-plant-uml")]
public class SolutionCreateFromPlantUmlRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('p')]
    public string PlantUmlSourcePath { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SolutionCreateFromPlantUmlRequestHandler : IRequestHandler<SolutionCreateFromPlantUmlRequest>
{
    private readonly ILogger<SolutionCreateFromPlantUmlRequestHandler> logger;
    private readonly ISolutionService solutionService;
    private readonly IClipboardService clipboardService;
    private readonly ICommandService commandService;

    public SolutionCreateFromPlantUmlRequestHandler(
        ILogger<SolutionCreateFromPlantUmlRequestHandler> logger,
        ISolutionService solutionService,
        IClipboardService clipboardService,
        ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.clipboardService = clipboardService ?? throw new ArgumentNullException(nameof(clipboardService));
        this.solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(SolutionCreateFromPlantUmlRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(SolutionCreateFromPlantUmlRequestHandler));
    }
}
