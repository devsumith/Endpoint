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
    private readonly ILogger<BenchmarkCreateRequestHandler> logger;
    private readonly ISolutionFactory solutionFactory;
    private readonly ISolutionService solutionService;
    private readonly ICommandService commandService;

    public BenchmarkCreateRequestHandler(
        ILogger<BenchmarkCreateRequestHandler> logger,
        ISolutionService solutionService,
        ISolutionFactory solutionFactory,
        ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        this.solutionFactory = solutionFactory ?? throw new ArgumentNullException(nameof(solutionFactory));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(BenchmarkCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(BenchmarkCreateRequestHandler));

        var model = await solutionFactory.Create(request.Name, request.ProjectName, "benchmark", request.FolderName, request.Directory);

        await solutionService.Create(model);

        commandService.Start($"start {model.SolultionFileName}", model.SolutionDirectory);
    }
}
