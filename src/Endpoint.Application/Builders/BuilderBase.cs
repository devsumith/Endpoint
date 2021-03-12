using Endpoint.Application.Models;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;

namespace Endpoint.Application.Builders
{
    public class BuilderBase<T>
        where T: class
    {
        protected readonly ICommandService _commandService;
        protected readonly ITemplateProcessor _templateProcessor;
        protected readonly ITemplateLocator _templateLocator;
        protected readonly IFileSystem _fileSystem;
        
        protected Settings _options;
        protected Token _directory = (Token)System.Environment.CurrentDirectory;
        protected Token _rootNamespace;
        protected Token _namespace;
        protected Token _domainNamespace;
        protected Token _applicationNamespace;

        public BuilderBase(
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

        public T WithSettings(Settings options)
        {
            _options = options;
            return this as T;
        }

        public T SetDirectory(string directory)
        {
            _directory = (Token)directory;
            return this as T;
        }

        public T SetRootNamespace(string rootNamespace)
        {
            _rootNamespace = (Token)rootNamespace;
            return this as T;
        }

        public T SetNamespace(string entityName)
        {
            _namespace = (Token)entityName;
            return this as T;
        }

        public T SetDomainNamespace(string domainNamespace)
        {
            _domainNamespace = (Token)domainNamespace;
            return this as T;
        }

        public T SetApplicationNamespace(string applicationNamespace)
        {
            _applicationNamespace = (Token)applicationNamespace;
            return this as T;
        }
    }
}
