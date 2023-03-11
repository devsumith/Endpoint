// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Projects;
using Endpoint.Core.Models.Artifacts.Projects.Factories;
using Endpoint.Core.Models.Artifacts.Projects.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("project-add")]
public class ProjectAddRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('t')]
    public string DotNetProjectType { get; set; } = "classlib";

    [Option('f')]
    public string FolderName { get; set; }

    [Option('r')]
    public string References { get; set; }

    [Option('m',"metadata")]
    public string Metadata { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class ProjectAddRequestHandler : IRequestHandler<ProjectAddRequest>
{
    private readonly ILogger<ProjectAddRequestHandler> _logger;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly ICommandService _commandService;
    private readonly IFileSystem _fileSystem;
    private readonly IProjectService _projectService;
    private readonly IProjectModelFactory _projectModelFactory;
    private readonly IFileProvider _fileProvider;
    public ProjectAddRequestHandler(
        ILogger<ProjectAddRequestHandler> logger,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        ICommandService commandService,
        IFileSystem fileSystem,
        IProjectService projectService,
        IProjectModelFactory projectModelFactory,
        IFileProvider fileProvider
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        _projectModelFactory = projectModelFactory ?? throw new ArgumentNullException(nameof(projectModelFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    public async Task Handle(ProjectAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ProjectAddRequestHandler));

        var projectPath = _fileProvider.Get("*.csproj", request.Directory);

        var projectDirectory = Path.GetDirectoryName(projectPath);

        var projectName = Path.GetFileNameWithoutExtension(projectPath);

        if (string.IsNullOrEmpty(request.FolderName))
        {
            _fileSystem.CreateDirectory($"{request.Directory}{Path.DirectorySeparatorChar}{request.FolderName}");
        }

        var directory = string.IsNullOrEmpty(request.FolderName) ? request.Directory : $"{request.Directory}{Path.DirectorySeparatorChar}{request.FolderName}";

        if(string.IsNullOrEmpty(request.Name))
        {
            _projectService.AddToSolution(new ProjectModel
            {
                Name = projectName,
                Directory = projectDirectory
            });

            return;
        }

        var model = _projectModelFactory.Create(request.DotNetProjectType, request.Name, projectDirectory, request.References?.Split(',').ToList(), request.Metadata);

        _projectService.AddProject(model);
    }
}
