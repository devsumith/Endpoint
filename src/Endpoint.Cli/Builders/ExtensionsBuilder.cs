using Endpoint.Cli.Services;
using Endpoint.Cli.ValueObjects;

namespace Endpoint.Cli.Builders
{
    public class ExtensionsBuilder: BuilderBase<ExtensionsBuilder>
    {
        public ExtensionsBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem):base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        private Token _entityName;
        public ExtensionsBuilder WithEntity(string entity)
        {
            _entityName = (Token)entity;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(ExtensionsBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .With(nameof(_entityName), _entityName)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/Extensions.cs", contents);
        }
    }
}
