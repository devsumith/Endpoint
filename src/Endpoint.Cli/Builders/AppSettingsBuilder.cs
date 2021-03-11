using Endpoint.Cli.Services;
using Endpoint.Cli.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Cli.Builders
{
    public class AppSettingsBuilder
    {
        private readonly ICommandService _commandService;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;
        private readonly IFileSystem _fileSystem;
        private readonly INamingConventionConverter _namingConventionConverter;

        private Token _directory = (Token)System.Environment.CurrentDirectory;
        private Token _rootNamespace;
        
        public AppSettingsBuilder(
            ICommandService commandService,
            ITokenBuilder tokenBuilder,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem,
            INamingConventionConverter namingConventionConverter)
        {
            _commandService = commandService;
            _tokenBuilder = tokenBuilder;
            _templateProcessor = templateProcessor;
            _templateLocator = templateLocator;
            _fileSystem = fileSystem;
            _namingConventionConverter = namingConventionConverter;
        }

        public AppSettingsBuilder SetDirectory(string directory)
        {
            _directory = (Token)directory;
            return this;
        }

        public AppSettingsBuilder SetRootNamespace(string rootNamespace)
        {
            _rootNamespace = (Token)rootNamespace;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(AppSettingsBuilder));

            var tokens = new SimpleTokensBuilder()
                .WithToken(nameof(_rootNamespace), _rootNamespace)
                .WithToken(nameof(_directory), _rootNamespace)
                .Build();
                
            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory}/{((Token)nameof(AppSettingsBuilder)).PascalCase}.cs", contents);
        }
    }
}
