// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Endpoint.Cli.Commands;


[Verb("benchmark-create")]
public class BenchmarkCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; } = "Library";

    [Option('p')]
    public string ProjectName { get; set; } = "Benchmarks";

    [Option('f')]
    public string FolderName { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class BenchmarkCreateRequestHandler : IRequestHandler<BenchmarkCreateRequest>
{
    private readonly ILogger<BenchmarkCreateRequestHandler> _logger;
    private readonly ISolutionModelFactory _solutionModelFactory;
    private readonly ISolutionService _solutionService;
    private readonly ICommandService _commandService;

    public BenchmarkCreateRequestHandler(
        ILogger<BenchmarkCreateRequestHandler> logger,
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

    public async Task Handle(BenchmarkCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(BenchmarkCreateRequestHandler));

        var model = await _solutionModelFactory.Create(request.Name, request.ProjectName, "benchmark", request.FolderName, request.Directory);

        await _solutionService.Create(model);

        _commandService.Start($"start {model.SolultionFileName}", model.SolutionDirectory);

    }
}
