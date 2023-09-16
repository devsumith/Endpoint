// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Linq;
using Endpoint.Core.Artifacts.Folders;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Services;

namespace Endpoint.Core.Artifacts.Solutions.Strategies;

public class SolutionGenerationStrategy : GenericArtifactGenerationStrategy<SolutionModel>
{
    private readonly ICommandService commandService;
    private readonly IFileSystem fileSystem;
    private readonly IProjectService projectService;

    public SolutionGenerationStrategy(ICommandService commandService, IFileSystem fileSystem, IProjectService projectService)
    {
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
    }

    public override async Task GenerateAsync(IArtifactGenerator generator, SolutionModel model)
    {
        fileSystem.Directory.CreateDirectory(model.SolutionDirectory);

        commandService.Start($"dotnet new sln -n {model.Name}", model.SolutionDirectory);

        await CreateProjectsAndAddToSln(generator, model, model.Folders);

        foreach (var dependOn in model.DependOns)
        {
            commandService.Start($"dotnet add {dependOn.Client.Directory} reference {dependOn.Service.Path}");
        }
    }

    private async Task CreateProjectsAndAddToSln(IArtifactGenerator artifactGenerator, SolutionModel model, List<FolderModel> folders)
    {
        foreach (var folder in folders)
        {
            foreach (var project in folder.Projects.OrderBy(x => x.Order))
            {
                fileSystem.Directory.CreateDirectory(folder.Directory);

                await artifactGenerator.GenerateAsync(project);

                await projectService.AddToSolution(project);
            }

            await CreateProjectsAndAddToSln(artifactGenerator, model, folder.SubFolders);
        }
    }
}
