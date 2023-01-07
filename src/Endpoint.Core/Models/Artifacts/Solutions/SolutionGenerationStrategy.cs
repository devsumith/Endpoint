using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Projects;
using Endpoint.Core.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Models.Artifacts.Solutions;

public class SolutionGenerationStrategy : ArtifactGenerationStrategyBase<SolutionModel>
{
    private readonly ICommandService _commandService;
    private readonly IFileSystem _fileSystem;

    public SolutionGenerationStrategy(IServiceProvider serviceProvider, ICommandService commandService, IFileSystem fileSystem) 
        : base(serviceProvider)
    {
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));    
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, SolutionModel model, dynamic configuration = null)
    {
        _fileSystem.CreateDirectory(model.SolutionDirectory);

        _commandService.Start($"dotnet new sln -n {model.Name}", model.SolutionDirectory);

        CreateProjectsAndAddToSln(artifactGenerationStrategyFactory, model, model.Folders);

        foreach (var dependOn in model.DependOns)
        {
            _commandService.Start($"dotnet add {dependOn.Client.Directory} reference {dependOn.Service.Path}");
        }
    }


    private void CreateProjectsAndAddToSln(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, SolutionModel model, List<FolderModel> folders)
    {
        foreach (var folder in folders)
        {
            foreach(var project in folder.Projects)
            {
                _fileSystem.CreateDirectory(folder.Directory);

                artifactGenerationStrategyFactory.CreateFor(project);

                _commandService.Start($"dotnet sln {model.SolultionFileName} add {project.Path}", model.SolutionDirectory);
            }

            CreateProjectsAndAddToSln(artifactGenerationStrategyFactory, model, folder.SubFolders);
        }
    }
}
