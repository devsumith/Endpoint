// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Artifacts.Projects;
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

    [Option('m', "metadata")]
    public string Metadata { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class ProjectAddRequestHandler : IRequestHandler<ProjectAddRequest>
{
    private readonly ILogger<ProjectAddRequestHandler> _logger;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly ICommandService _commandService;
    private readonly IFileSystem _fileSystem;
    private readonly IProjectService _projectService;
    private readonly IProjectFactory _projectFactory;
    private readonly IFileProvider _fileProvider;
    public ProjectAddRequestHandler(
        ILogger<ProjectAddRequestHandler> logger,
        IArtifactGenerator artifactGenerator,
        ICommandService commandService,
        IFileSystem fileSystem,
        IProjectService projectService,
        IProjectFactory projectFactory,
        IFileProvider fileProvider
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        _projectFactory = projectFactory ?? throw new ArgumentNullException(nameof(projectFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    public async Task Handle(ProjectAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ProjectAddRequestHandler));

        if (string.IsNullOrEmpty(request.Name))
        {
            ProjectAdd(request.Directory);
            return;
        }

        var projectPath = Path.Combine(request.Directory, request.Name);

        var projectDirectory = Path.GetDirectoryName(projectPath);

        if (string.IsNullOrEmpty(request.FolderName))
        {
            _fileSystem.CreateDirectory($"{request.Directory}{Path.DirectorySeparatorChar}{request.FolderName}");
        }

        var model = await _projectFactory.Create(request.DotNetProjectType, request.Name, projectDirectory, request.References?.Split(',').ToList(), request.Metadata);

        await _projectService.AddProjectAsync(model);
    }

    public void ProjectAdd(string directory)
    {
        var projectPath = _fileProvider.Get("*.csproj", directory);

        if (projectPath != Constants.FileNotFound)
        {
            var projectName = Path.GetFileNameWithoutExtension(projectPath);

            var projectDirectory = Path.GetDirectoryName(projectPath);

            _projectService.AddToSolution(new ProjectModel
            {
                Name = projectName,
                Directory = projectDirectory
            });
        }

        foreach (var subDirectory in Directory.GetDirectories(directory))
        {
            var subDirectoryName = Path.GetFileName(subDirectory);

            var invalidDirectories = new[]
            {
                ".git",
                "node_modules",
                "bin",
                "obj",
                "Properties",
                "nupkg",
                "dist",
                ".angular",
                ".vs",
                ".vscode"
            };

            if (!invalidDirectories.Contains(subDirectoryName))
            {
                ProjectAdd(subDirectory);
            }
        }
    }
}
