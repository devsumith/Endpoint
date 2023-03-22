// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Services;
using System.IO;
using Endpoint.Core.Models.Artifacts.Projects;
using System.Linq;
using Endpoint.Core;
using Endpoint.Core.Models.Artifacts.Projects.Services;

namespace Endpoint.Cli.Commands;


[Verb("project-move")]
public class ProjectMoveRequest : IRequest {
    [Option("dest", Required = true)]
    public string DestinationRelativePath { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ProjectMoveRequestHandler : IRequestHandler<ProjectMoveRequest>
{
    private readonly ILogger<ProjectMoveRequestHandler> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly ICommandService _commandService;
    private readonly IProjectService _projectService;

    public ProjectMoveRequestHandler(
        ILogger<ProjectMoveRequestHandler> logger,
        ICommandService commandService,
        IFileProvider fileProvider,
        IProjectService projectService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
    }

    public async Task Handle(ProjectMoveRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ProjectMoveRequestHandler));

        var destinationDirectoryRoot = Path.GetFullPath(request.DestinationRelativePath, request.Directory);

        Remove(request.Directory);

        if (!Directory.Exists(destinationDirectoryRoot))
        {
            Directory.CreateDirectory(destinationDirectoryRoot);
        }

        var destinationDirectory = Path.Combine(destinationDirectoryRoot, Path.GetFileName(request.Directory));

        Directory.Move(request.Directory, destinationDirectory);

        Add(destinationDirectory);

    }

    public void Add(string directory)
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
                Add(subDirectory);
            }
        }
    }

    public void Remove(string directory)
    {
        var solutionPath = _fileProvider.Get("*.sln", directory);

        var solutionDirectory = Path.GetDirectoryName(solutionPath);

        var solutionFileName = Path.GetFileName(solutionPath);

        foreach (var path in Directory.GetFiles(directory, "*.csproj", SearchOption.AllDirectories))
        {
            _commandService.Start($"dotnet sln {solutionFileName} remove {path}", solutionDirectory);
        }
    }
}
