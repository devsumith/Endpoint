using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System;

namespace Endpoint.Application.Builders
{
    public class ModelBuilder: BuilderBase<ModelBuilder>
    {
        private Token _entityName;

        public ModelBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem):base(commandService,templateProcessor,templateLocator,fileSystem)
        { }

        public ModelBuilder SetEntityName(string entityName)
        {
            _entityName = (Token)entityName;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(ModelBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .With(nameof(_entityName), _entityName)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/{_entityName.PascalCase}.cs", contents);
        }
    }
}
