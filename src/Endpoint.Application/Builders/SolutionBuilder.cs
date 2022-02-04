using Endpoint.Application.Models;
using Endpoint.SharedKernal.Models;
using Endpoint.SharedKernal.Services;

namespace Endpoint.Application.Builders
{
    public interface ISolutionBuilder
    {
        SolutionReference Build(Endpoint.SharedKernal.Models.Settings settings);
    }

    public class SolutionBuilder : ISolutionBuilder
    {
        protected readonly ICommandService _commandService;
        protected readonly ITemplateProcessor _templateProcessor;
        protected readonly ITemplateLocator _templateLocator;
        protected readonly IFileSystem _fileSystem;

        public SolutionBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem)
        {
            _commandService = commandService;
            _templateProcessor = templateProcessor;
            _templateLocator = templateLocator;
            _fileSystem = fileSystem;
        }

        public SolutionReference Build(Settings settings)
        {
            _commandService.Start($"mkdir {settings.SolutionName}", settings.RootDirectory);

            _commandService.Start($"dotnet new sln -n {settings.SolutionName}", settings.RootDirectory);

            _commandService.Start($"mkdir {settings.SourceFolder}", settings.RootDirectory);

            return new(_commandService, settings.RootDirectory, settings.SolutionName);
        }
    }
}
