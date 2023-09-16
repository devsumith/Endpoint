// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Folders;
using Endpoint.Core.Artifacts.Projects;
using Endpoint.Core.Artifacts.Projects.Enums;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Artifacts.Solutions.Factories;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("background-processor-create")]
public class BackgroundProcessorCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class BackgroundProcessorCreateRequestHandler : IRequestHandler<BackgroundProcessorCreateRequest>
{
    private readonly ILogger<BackgroundProcessorCreateRequestHandler> logger;
    private readonly ISolutionFactory solutionFactory;
    private readonly IProjectFactory projectFactory;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly ICommandService commandService;

    public BackgroundProcessorCreateRequestHandler(
        ILogger<BackgroundProcessorCreateRequestHandler> logger,
        ISolutionFactory solutionFactory,
        IProjectFactory projectFactory,
        IArtifactGenerator artifactGenerator,
        ICommandService commandService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.solutionFactory = solutionFactory ?? throw new ArgumentNullException(nameof(solutionFactory));
        this.projectFactory = projectFactory ?? throw new ArgumentNullException(nameof(projectFactory));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public async Task Handle(BackgroundProcessorCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogDebug("Handled: {0}", nameof(BackgroundProcessorCreateRequestHandler));

        var model = new SolutionModel(request.Name, request.Directory);

        var src = new FolderModel("src", model.SolutionDirectory);

        var core = new ProjectModel(DotNetProjectType.ClassLib, $"{request.Name}.Core", src.Directory);

        var backgroundProcessor = new ProjectModel(DotNetProjectType.Worker, $"{request.Name}.BackgroundProcessor", src.Directory);

        backgroundProcessor.References.Add(Path.Combine("..", core.Name, $"{core.Name}.csproj"));

        src.Projects.Add(core);

        src.Projects.Add(backgroundProcessor);

        model.Folders.Add(src);

        await artifactGenerator.GenerateAsync(model);

        commandService.Start($"start {model.SolutionPath}");
    }
}
