using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;

namespace Endpoint.Application.Builders
{
    public class ControllerTestBuilder: BuilderBase<ControllerTestBuilder>
    {
        private string _entity;

        public ControllerTestBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem):base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        
        public void Build()
        {
            var template = _templateLocator.Get(nameof(ControllerTestBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/{((Token)_entity).PascalCase}ControllerTests.cs", contents);
        }
    }
}
