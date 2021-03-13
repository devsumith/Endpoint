using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;

namespace Endpoint.Application.Builders.CSharp
{
    public class NamespaceBuilder : BuilderBase<NamespaceBuilder>
    {
        public NamespaceBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }


        public void Build()
        {
            var template = _templateLocator.Get(nameof(NamespaceBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/Namespace.cs", contents);
        }
    }
}
