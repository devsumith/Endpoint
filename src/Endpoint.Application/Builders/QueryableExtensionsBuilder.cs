using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class QueryableExtensionsBuilder : BuilderBase<QueryableExtensionsBuilder>
    {
        public QueryableExtensionsBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }


        public void Build()
        {
            var template = _templateLocator.Get(nameof(QueryableExtensionsBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With(nameof(_namespace), _namespace)
                .With(nameof(_applicationNamespace), _applicationNamespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}{Path.DirectorySeparatorChar}QueryableExtensions.cs", contents);
        }
    }
}
