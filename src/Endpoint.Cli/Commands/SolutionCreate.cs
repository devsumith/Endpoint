// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.Solutions.Factories;
using Endpoint.Core.Artifacts.Solutions.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;


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
    private readonly ILogger<SolutionCreateRequestHandler> _logger;
    private readonly ISolutionFactory _solutionFactory;
    private readonly ISolutionService _solutionService;
    private readonly ICommandService _commandService;

    public SolutionCreateRequestHandler(
        ILogger<SolutionCreateRequestHandler> logger,
        ISolutionService solutionService,
        ISolutionFactory solutionFactory,
        ICommandService commandService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _solutionFactory = solutionFactory ?? throw new ArgumentNullException(nameof(solutionFactory));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(SolutionCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SolutionCreateRequestHandler));

        var model = await _solutionFactory.Create(request.Name, request.ProjectName, request.ProjectType, request.FolderName, request.Directory);

        if (request.NoServiceCreate)
            model.RemoveAllServices();

        await _solutionService.Create(model);

        _commandService.Start($"start {model.SolultionFileName}", model.SolutionDirectory);

    }
}
