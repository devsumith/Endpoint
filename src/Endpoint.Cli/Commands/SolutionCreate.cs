// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Solutions.Factories;
using Endpoint.Core.Artifacts.Solutions.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("solution-create")]
public class SolutionCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('p')]
    public string ProjectName { get; set; } = "Worker.Console";

    [Option('f')]
    public string FolderName { get; set; }

    [Option("no-service-create")]
    public bool NoServiceCreate { get; set; }

    [Option('t')]
    public string ProjectType { get; set; } = "worker";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SolutionCreateRequestHandler : IRequestHandler<SolutionCreateRequest>
{
    private readonly ILogger<SolutionCreateRequestHandler> logger;
    private readonly ISolutionFactory solutionFactory;
    private readonly ISolutionService solutionService;
    private readonly ICommandService commandService;

    public SolutionCreateRequestHandler(
        ILogger<SolutionCreateRequestHandler> logger,
        ISolutionService solutionService,
        ISolutionFactory solutionFactory,
        ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        this.solutionFactory = solutionFactory ?? throw new ArgumentNullException(nameof(solutionFactory));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(SolutionCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(SolutionCreateRequestHandler));

        var model = await solutionFactory.Create(request.Name, request.ProjectName, request.ProjectType, request.FolderName, request.Directory);

        if (request.NoServiceCreate)
        {
            model.RemoveAllServices();
        }

        await solutionService.Create(model);

        commandService.Start($"start {model.SolultionFileName}", model.SolutionDirectory);
    }
}
