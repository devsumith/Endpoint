using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;

namespace Endpoint.Application.Builders
{
    public class ResetDbBuilder : BuilderBase<ResetDbBuilder>
    {
        public ResetDbBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }


        public void Build()
        {
            var template = _templateLocator.Get(nameof(ResetDbBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/ResetDb.cs", contents);
        }
    }
}
