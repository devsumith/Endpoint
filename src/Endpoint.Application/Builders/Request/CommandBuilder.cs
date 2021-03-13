using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;

namespace Endpoint.Application.Builders
{
    public class CommandBuilder : BuilderBase<CommandBuilder>
    {
        private Token _name;
        private Token _entityName;
        private Token _dbContext;
        private Token _buildingBlocksCoreNamespace;
        private Token _buildingBlocksEventStoreNamespace;
        private Token _store;

        public CommandBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public CommandBuilder WithBuildingBlocksCoreNamespace(string buildingBlocksCoreNamespace)
        {
            _buildingBlocksCoreNamespace = (Token)buildingBlocksCoreNamespace;
            return this;
        }

        public CommandBuilder WithBuildingBlocksEventStoreNamespace(string buildingBlocksEventStoreNamespace)
        {
            _buildingBlocksEventStoreNamespace = (Token)buildingBlocksEventStoreNamespace;
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

        public CommandBuilder WithStore(string store)
        {
            _store = (Token)store;
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
                .With(nameof(_namespace), _namespace)
                .With(nameof(_buildingBlocksCoreNamespace), _buildingBlocksCoreNamespace)
                .With(nameof(_buildingBlocksEventStoreNamespace), _buildingBlocksEventStoreNamespace)
                .With(nameof(_applicationNamespace), _applicationNamespace)
                .With(nameof(_domainNamespace), _domainNamespace)
                .With(nameof(_store), _store)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/{_name.PascalCase}.cs", contents);
        }
    }
}
