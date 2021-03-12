using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;

namespace Endpoint.Application.Builders
{
    public class ControllerBuilder: BuilderBase<ControllerBuilder>
    {
        public ControllerBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem):base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        private Token _resource;
        
        public ControllerBuilder SetResource(string resource)
        {
            _resource = (Token)resource;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(ControllerBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .With(nameof(_resource), _resource)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/Controller.cs", contents);
        }
    }
}
