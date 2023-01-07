using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Projects;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands;


[Verb("project-add")]
public class ProjectAddRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Name { get; set; }

    [Option('t')]
    public string DotNetProjectType { get; set; }

    [Option('f')]
    public string FolderName { get; set; }

    [Option('r')]
    public string References { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class ProjectAddRequestHandler : IRequestHandler<ProjectAddRequest, Unit>
{
    private readonly ILogger<ProjectAddRequestHandler> _logger;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly ICommandService _commandService;
    private readonly IFileSystem _fileSystem;
    private readonly IProjectService _projectService;

    public ProjectAddRequestHandler(
        ILogger<ProjectAddRequestHandler> logger,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        ICommandService commandService,
        IFileSystem fileSystem,
        IProjectService projectService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
    }

    public async Task<Unit> Handle(ProjectAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ProjectAddRequestHandler));

        if (string.IsNullOrEmpty(request.FolderName))
        {
            _fileSystem.CreateDirectory($"{request.Directory}{Path.DirectorySeparatorChar}{request.FolderName}");
        }

        var directory = string.IsNullOrEmpty(request.FolderName) ? request.Directory : $"{request.Directory}{Path.DirectorySeparatorChar}{request.FolderName}";

        var model = new ProjectModel(request.DotNetProjectType, request.Name, directory, request.References.Split(',').ToList());

        _projectService.AddProject(model);

        return new();
    }
}