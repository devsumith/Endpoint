using Endpoint.Cli.Services;
using Endpoint.Cli.ValueObjects;

namespace Endpoint.Cli.Builders
{
    public class CommandBuilder: BuilderBase<CommandBuilder>
    {
        private readonly ICommandService _commandService;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;
        private readonly IFileSystem _fileSystem;

        private Token _directory = (Token)System.Environment.CurrentDirectory;
        private Token _rootNamespace;
        private Token _name;
        private Token _entityName;
        private Token _dbContext;

        public CommandBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem): base(commandService, templateProcessor, templateLocator, fileSystem)
        {
            _commandService = commandService;
            _templateProcessor = templateProcessor;
            _templateLocator = templateLocator;
            _fileSystem = fileSystem;
        }

        public CommandBuilder SetDirectory(string directory)
        {
            _directory = (Token)directory;
            return this;
        }

        public CommandBuilder SetRootNamespace(string rootNamespace)
        {
            _rootNamespace = (Token)rootNamespace;
            return this;
        }

        public CommandBuilder WithDbContext(string dbContext)
        {
            _dbContext = (Token)dbContext;
            return this;
        }

        public CommandBuilder WithEntity(string entity)
        {
            _entityName = (Token)entity;
            return this;
        }

        public CommandBuilder WithName(string name)
        {
            _name = (Token)name;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(CommandBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_entityName), _entityName)
                .With(nameof(_name), _name)
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_dbContext), _dbContext)
                .With("Namespace", (Token)$"{_rootNamespace.Value}.Api.Features")
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/{_name.PascalCase}.cs", contents);
        }
    }
}
