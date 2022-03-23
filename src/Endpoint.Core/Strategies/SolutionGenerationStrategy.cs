using Endpoint.Core.Models;
using Endpoint.Core.Services;
using System;
using System.Linq;

namespace Endpoint.Core.Strategies
{

    public class SolutionGenerationStrategy: ISolutionGenerationStrategy
    {
        private readonly IProjectGenerationStrategy _projectGenerationStrategy;
        private readonly ICommandService _commandService;
        private readonly IFileSystem _fileSystem;

        public SolutionGenerationStrategy(
            IFileSystem fileSystem,
            ICommandService commandService,
            IProjectGenerationStrategy projectGenerationStrategy
            )
        {
            _commandService = commandService;
            _projectGenerationStrategy = projectGenerationStrategy;
            _fileSystem = fileSystem;
        }

        public void Create(SolutionModel model)
        {
            _fileSystem.CreateDirectory(model.SolutionDirectory);

            _commandService.Start($"dotnet new sln -n {model.Name}", model.SolutionDirectory);

            _fileSystem.CreateDirectory(model.SrcDirectory);

            _fileSystem.CreateDirectory(model.TestDirectory);

            foreach (var project in model.Projects.OrderByDescending(x => x.Order))
            {
                CreateProjectAndAddToSolution(project.DotNetProjectType, model.SolutionDirectory, project.Path, project.Directory);
            }

            foreach (var project in model.Projects)
            {
                _projectGenerationStrategy.Create(project);
            }

            foreach(var dependOn in model.DependOns)
            {
                _commandService.Start($"dotnet add {dependOn.Client.Directory} reference {dependOn.Supplier.Path}");
            }
        }

        private void CreateProjectAndAddToSolution(DotNetProjectType dotNetProjectType, string directory, string projectPath, string projectDirectory)
        {
            _fileSystem.CreateDirectory(projectDirectory);

            var templateType = dotNetProjectType switch
            {
                DotNetProjectType.Console => "console",
                DotNetProjectType.XUnit => "xunit",
                DotNetProjectType.ClassLib => "classlib",
                DotNetProjectType.WebApi => "webapi",
                DotNetProjectType.MinimalWebApi => "webapi -minimal",
                _ => throw new NotImplementedException(),
            };

            _commandService.Start($"dotnet new {templateType} --framework net6.0", projectDirectory);

            _commandService.Start($"dotnet sln add {projectPath}", directory);
        }

    }
}
