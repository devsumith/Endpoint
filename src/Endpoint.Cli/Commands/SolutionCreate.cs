                                                                                                                                                                                                                // Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Endpoint.Cli.Commands;


[Verb("solution-create")]
public class SolutionCreateRequest : IRequest {
    [Option('n',"name")]
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
    private readonly ISolutionModelFactory _solutionModelFactory;
    private readonly ISolutionService _solutionService;
    private readonly ICommandService _commandService;

    public SolutionCreateRequestHandler(
        ILogger<SolutionCreateRequestHandler> logger,
        ISolutionService solutionService,
        ISolutionModelFactory solutionModelFactory,
        ICommandService commandService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _solutionModelFactory = solutionModelFactory ?? throw new ArgumentNullException(nameof(solutionModelFactory));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task<Unit> Handle(SolutionCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SolutionCreateRequestHandler));

        var model = _solutionModelFactory.Create(request.Name, request.ProjectName, request.ProjectType, request.FolderName, request.Directory);

        if(request.NoServiceCreate)
            model.RemoveAllServices();

        _solutionService.Create(model);

        _commandService.Start($"start {model.SolultionFileName}", model.SolutionDirectory);

        return new();
    }
}
