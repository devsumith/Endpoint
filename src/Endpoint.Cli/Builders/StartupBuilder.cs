using Endpoint.Cli.Services;
using System.Collections.Generic;

namespace Endpoint.Cli.Builders
{
    public class StartupBuilder
    {
        private readonly ICommandService _commandService;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;
        private readonly IFileSystem _fileSystem;
        private readonly INamingConventionConverter _namingConventionConverter;

        private string _directory = System.Environment.CurrentDirectory;
        private string _rootNamespace;
        
        public StartupBuilder(
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

        public StartupBuilder SetDirectory(string directory)
        {
            _directory = directory;
            return this;
        }

        public StartupBuilder SetRootNamespace(string rootNamespace)
        {
            _rootNamespace = rootNamespace;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(StartupBuilder ));

            var tokens = _tokenBuilder.Build(new Dictionary<string, string> {
                    { "RootNamespace", _rootNamespace }
                }, _directory);

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory}/{_namingConventionConverter.Convert(NamingConvention.PascalCase,nameof(StartupBuilder))}.cs", contents);
        }
    }
}
