using Endpoint.Cli.Services;
using Endpoint.Cli.ValueObjects;

namespace Endpoint.Cli.Builders
{
    public class DependenciesBuilder
    {
        private readonly ICommandService _commandService;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;
        private readonly IFileSystem _fileSystem;

        private Token _directory = (Token)System.Environment.CurrentDirectory;
        private Token _rootNamespace;
        
        public DependenciesBuilder(
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

        public DependenciesBuilder SetDirectory(string directory)
        {
            _directory = (Token)directory;
            return this;
        }

        public DependenciesBuilder SetRootNamespace(string rootNamespace)
        {
            _rootNamespace = (Token)rootNamespace;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(DependenciesBuilder ));

            var tokens = new SimpleTokensBuilder()
                .WithToken(nameof(_rootNamespace), _rootNamespace)
                .WithToken(nameof(_directory), _directory)
                .WithToken("Namespace", (Token)$"{_rootNamespace.Value}.Api")
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/Dependencies.cs", contents);
        }
    }
}
