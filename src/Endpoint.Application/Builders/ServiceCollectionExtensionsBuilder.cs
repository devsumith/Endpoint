using Endpoint.Application.Services;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class ServiceCollectionExtensionsBuilder: BuilderBase<ServiceCollectionExtensionsBuilder>
    {
        public ServiceCollectionExtensionsBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem): base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(ServiceCollectionExtensionsBuilder ));

            var tokens = new TokensBuilder()
                .With(nameof(_applicationNamespace), _applicationNamespace)
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}{Path.DirectorySeparatorChar}ServiceCollectionExtensions.cs", contents);
        }
    }
}
