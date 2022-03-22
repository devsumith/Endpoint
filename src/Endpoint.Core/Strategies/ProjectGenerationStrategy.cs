using Endpoint.Core.Managers;
using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Endpoint.Core.Strategies
{
    public class ProjectGenerationStrategy
    {
        private readonly FileGenerationStrategy _fileGenerationStrategy;
        private readonly ICommandService _commandService;
        private readonly IFileSystem _fileSystem;

        public ProjectGenerationStrategy(
            IFileSystem fileSystem,
            ITemplateLocator templateLocator,
            ITemplateProcessor templateProcessor,
            ILogger logger,
            ICommandService commandService,
            ISolutionNamespaceProvider solutionNamespaceProvider
            )
        {
            _commandService = commandService;
            _fileGenerationStrategy = new(fileSystem, templateLocator, templateProcessor, solutionNamespaceProvider, logger);
            _fileSystem = fileSystem;
        }

        public void Create(ProjectModel model)
        {
            foreach(var path in _fileSystem.GetFiles(model.Directory,"*.cs",SearchOption.AllDirectories))
            {
                _fileSystem.Delete(path);
            }

            foreach(var package in model.Packages)
            {
                _commandService.Start($"dotnet add package {package.Name} --version {package.Version}",model.Directory);
            }

            foreach(var file in model.Files)
            {
                _fileGenerationStrategy.Create(file);
            }
        }
    }
}
