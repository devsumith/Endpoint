using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Endpoint.Application.Commands;


[Verb("solution-create")]
public class SolutionCreateRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Name { get; set; }

    [Option('p')]
    public string ProjectName { get; set; } = "Worker.Console";

    [Option('t')]
    public string ProjectType { get; set; } = "worker";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SolutionCreateRequestHandler : IRequestHandler<SolutionCreateRequest, Unit>
{
    private readonly ILogger<SolutionCreateRequestHandler> _logger;
    private readonly ISolutionModelFactory _solutionModelFactory;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly ICommandService _commandService;

    public SolutionCreateRequestHandler(
        ILogger<SolutionCreateRequestHandler> logger,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        ISolutionModelFactory solutionModelFactory,
        ICommandService commandService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _solutionModelFactory = solutionModelFactory ?? throw new ArgumentNullException(nameof(solutionModelFactory));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task<Unit> Handle(SolutionCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(SolutionCreateRequestHandler)}");

        var model = _solutionModelFactory.SingleProjectSolution(request.Name, request.ProjectName, request.ProjectType, request.Directory);

        _artifactGenerationStrategyFactory.CreateFor(model);

        _commandService.Start($"start {model.SolultionFileName}", model.SolutionDirectory);

        return new();
    }
}