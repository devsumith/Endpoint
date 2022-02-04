using Endpoint.SharedKernal.Services;

namespace Endpoint.Application.Builders
{
    public class ValidationBehaviorBuilder : BuilderBase<ValidationBehaviorBuilder>
    {
        public ValidationBehaviorBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }


        public void Build()
        {
            var template = _templateLocator.Get(nameof(ValidationBehaviorBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_applicationNamespace), _applicationNamespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/ValidationBehavior.cs", contents);
        }
    }
}
