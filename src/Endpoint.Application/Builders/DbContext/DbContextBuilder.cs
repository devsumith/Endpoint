using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class DbContextBuilder : BuilderBase<DbContextBuilder>
    {
        private Token _modelsDirectory;
        private Token _dbContext;
        private List<Token> _models = new List<Token>();

        public DbContextBuilder(
            ICommandService commandService,

            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public DbContextBuilder WithModel(string model)
        {
            _models.Add((Token)model);
            return this;
        }

        public DbContextBuilder SetModelsDirectory(string modelsDirectory)
        {
            _modelsDirectory = (Token)modelsDirectory;
            return this;
        }

        public DbContextBuilder WithDbContextName(string dbContextName)
        {
            _dbContext = (Token)dbContextName;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(DbContextBuilder));

            var interfaceTemplate = _templateLocator.Get("DbContextInterfaceBuilder");

            var tokens = new TokensBuilder()
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .With(nameof(_dbContext), _dbContext)
                .With(nameof(_domainNamespace), _domainNamespace)
                .With(nameof(_infrastructureNamespace), _infrastructureNamespace)
                .With(nameof(_applicationNamespace), _applicationNamespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            var interfaceContents = _templateProcessor.Process(interfaceTemplate, tokens);

            _fileSystem.WriteAllLines($@"{_infrastructureDirectory.Value}{Path.DirectorySeparatorChar}Data{Path.DirectorySeparatorChar}{_dbContext.PascalCase}.cs", contents);

            _fileSystem.WriteAllLines($@"{_applicationDirectory.Value}{Path.DirectorySeparatorChar}Interfaces{Path.DirectorySeparatorChar}I{_dbContext.PascalCase}.cs", interfaceContents);
        }
    }
}
