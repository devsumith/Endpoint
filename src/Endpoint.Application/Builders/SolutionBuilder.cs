using Endpoint.Application.Models;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class SolutionBuilder: BuilderBase<SolutionBuilder>
    {
        private string _slnDirectory => $"{_directory.Value}{Path.DirectorySeparatorChar}{_name}";
        private string _name;
        public SolutionBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem):base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public SolutionBuilder WithDirectory(string directory)
        {
            _directory = (Token)directory;
            return this;
        }

        public SolutionBuilder WithName(string name)
        {
            _name = name;
            return this;
        }
        public SolutionReference Build()
        {
            _commandService.Start($"mkdir {_name}", _directory.Value);

            _commandService.Start($"dotnet new sln -n {_name}", _slnDirectory);

            _commandService.Start($"mkdir src", _slnDirectory);

            return new(_commandService, _slnDirectory, _name);
        }
    }
}
