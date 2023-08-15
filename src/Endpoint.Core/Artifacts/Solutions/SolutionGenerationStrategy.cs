// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Folders;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Artifacts.Solutions;

public class SolutionGenerationStrategy : IArtifactGenerationStrategy<SolutionModel>
{
    private readonly ICommandService _commandService;
    private readonly IFileSystem _fileSystem;
    private readonly IProjectService _projectService;

    public SolutionGenerationStrategy(IServiceProvider serviceProvider, ICommandService commandService, IFileSystem fileSystem, IProjectService projectService)

    {
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
    }

    public async Task GenerateAsync(IArtifactGenerator artifactGenerator, SolutionModel model, dynamic context = null)
    {
        _fileSystem.Directory.CreateDirectory(model.SolutionDirectory);

        _commandService.Start($"dotnet new sln -n {model.Name}", model.SolutionDirectory);

        await CreateProjectsAndAddToSln(artifactGenerator, model, model.Folders);

        foreach (var dependOn in model.DependOns)
        {
            _commandService.Start($"dotnet add {dependOn.Client.Directory} reference {dependOn.Service.Path}");
        }
    }

    public int Priority { get; } = 0;
    private async Task CreateProjectsAndAddToSln(IArtifactGenerator artifactGenerator, SolutionModel model, List<FolderModel> folders)
    {
        foreach (var folder in folders)
        {
            foreach (var project in folder.Projects.OrderBy(x => x.Order))
            {
                _fileSystem.Directory.CreateDirectory(folder.Directory);

                await artifactGenerator.GenerateAsync(project);

                await _projectService.AddToSolution(project);
            }

            await CreateProjectsAndAddToSln(artifactGenerator, model, folder.SubFolders);
        }
    }
}

