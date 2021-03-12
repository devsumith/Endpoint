using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;

namespace Endpoint.Application.Builders
{
    public class ServiceCollectionExtensionsBuilder
    {
        private readonly ICommandService _commandService;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;
        private readonly IFileSystem _fileSystem;

        private Token _directory = (Token)System.Environment.CurrentDirectory;
        private Token _rootNamespace;
        
        public ServiceCollectionExtensionsBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem)
        {
            _commandService = commandService;
            _templateProcessor = templateProcessor;
            _templateLocator = templateLocator;
            _fileSystem = fileSystem;
        }

        public ServiceCollectionExtensionsBuilder SetDirectory(string directory)
        {
            _directory = (Token)directory;
            return this;
        }

        public ServiceCollectionExtensionsBuilder SetRootNamespace(string rootNamespace)
        {
            _rootNamespace = (Token)rootNamespace;
            return this;
        }

        public void Build()
        {
            var template = _templateLocator.Get(nameof(ServiceCollectionExtensionsBuilder ));

            var tokens = new TokensBuilder()
                .With(nameof(_rootNamespace), _rootNamespace)
                .With(nameof(_directory), _directory)
                .With("Namespace", (Token)$"{_rootNamespace.Value}.Api.Extensions")
                .Build();

            var contents = _templateProcessor.Process(template, tokens);

            _fileSystem.WriteAllLines($@"{_directory.Value}/{nameof(ServiceCollectionExtensionsBuilder)}.cs", contents);
        }
    }
}
