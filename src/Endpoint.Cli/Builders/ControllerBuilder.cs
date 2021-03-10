using Endpoint.Cli.Services;
using System;
using System.Collections.Generic;

namespace Endpoint.Cli.Builders
{
    public interface IControllerBuilder
    {

    }
    public class ControllerBuilder: IControllerBuilder
    {
        private readonly ICommandService _commandService;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;
        private readonly IFileSystem _fileSystem;

        private string _directory;
        private string _name;
        private string[] _template;
        private string _templateName = nameof(ControllerBuilder);

        public ControllerBuilder(
            ICommandService commandService,
            ITokenBuilder tokenBuilder,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem)
        {
            _commandService = commandService;
            _tokenBuilder = tokenBuilder;
            _templateProcessor = templateProcessor;
            _templateLocator = templateLocator;
            _fileSystem = fileSystem;
        }

        public ControllerBuilder SetDirectory(string directory)
        {
            _directory = directory;
            return this;
        }

        public ControllerBuilder SetName(string name)
        {
            _name = name;
            return this;
        }

        public void Build()
        {
            _template = _templateLocator.Get(_templateName);

            var tokens = _tokenBuilder.Build(new Dictionary<string, string> {
                    { "Name", $"{_name}" }
                }, _directory);

            var contents = _templateProcessor.Process(_template, tokens);

            _fileSystem.WriteAllLines("", contents);
        }
    }
}
