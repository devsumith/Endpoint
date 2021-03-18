using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System;

namespace Endpoint.Application.Builders
{
    public class QueryBuilder : BuilderBase<QueryBuilder>
    {
        private Token _name;
        private Token _entityName;
        private Token _dbContext;

        public QueryBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public QueryBuilder WithEntity(string entityName)
        {
            _entityName = (Token)entityName;
            return this;
        }

        public QueryBuilder WithName(string name)
        {
            _name = (Token)name;
            return this;
        }

        public QueryBuilder WithDbContext(string dbContext)
        {
            _dbContext = (Token)dbContext;
            return this;
        }

        public void Build()
        {
            try
            {
                var template = _templateLocator.Get(nameof(QueryBuilder));

                var tokens = new TokensBuilder()
                    .With(nameof(_entityName), _entityName)
                    .With(nameof(_name), _name)
                    .With(nameof(_applicationNamespace), _applicationNamespace)
                    .With(nameof(_directory), _directory)
                    .With(nameof(_domainNamespace), _domainNamespace)
                    .With(nameof(_dbContext), _dbContext)
                    .Build();

                var contents = _templateProcessor.Process(template, tokens);

                _fileSystem.WriteAllLines($@"{_applicationDirectory.Value}/Features/{_entityName.PascalCasePlural}/{_name.PascalCase}.cs", contents);
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
