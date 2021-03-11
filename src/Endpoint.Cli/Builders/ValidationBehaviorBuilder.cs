using Endpoint.Cli.Services;
using Endpoint.Cli.ValueObjects;

namespace Endpoint.Cli.Builders
{
    public class ValidationBehaviorBuilder
    {
        private readonly ICommandService _commandService;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;
        private readonly IFileSystem _fileSystem;


        private Token _directory = (Token)System.Environment.CurrentDirectory;
        private Token _rootNamespace;
        
        public ValidationBehaviorBuilder(
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

        public ValidationBehaviorBuilder SetDirectory(string directory)
        {
            _directory = (Token)directory;
            return this;
        }

        public ValidationBehaviorBuilder SetRootNamespace(string rootNamespace)
        {
            _rootNamespace = (Token)rootNamespace;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(ValidationBehaviorBuilder ));

            var tokens = new SimpleTokensBuilder()
                .WithToken("namespace", (Token)$"{_rootNamespace.Value}.Cli.Behaviors")
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/ValidationBehavior.cs", contents);
        }
    }
}
