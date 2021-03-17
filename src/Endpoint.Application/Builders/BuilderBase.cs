using Endpoint.Application.Models;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;

namespace Endpoint.Application.Builders
{
    public class BuilderBase<T>
        where T : class
    {
        protected readonly ICommandService _commandService;
        protected readonly ITemplateProcessor _templateProcessor;
        protected readonly ITemplateLocator _templateLocator;
        protected readonly IFileSystem _fileSystem;

        protected Settings _options;
        protected Token _directory = (Token)System.Environment.CurrentDirectory;

        protected Token _rootNamespace;
        protected Token _namespace;
        protected Token _apiNamespace;
        protected Token _domainNamespace;
        protected Token _applicationNamespace;
        protected Token _infrastructureNamespace;
        protected Token _domainDirectory;
        protected Token _infrastructureDirectory;
        protected Token _applicationDirectory;
        protected Token _apiDirectory;

        public BuilderBase()
        {

        }

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

        public TTo Map<TTo>(TTo builderBase)
            where TTo : BuilderBase<TTo>
        {
            builderBase.SetDomainDirectory(_domainDirectory?.Value);
            builderBase.SetApplicationDirectory(_applicationDirectory?.Value);
            builderBase.SetInfrastructureDirectory(_infrastructureDirectory?.Value);
            builderBase.SetApiDirectory(_apiDirectory?.Value);

            builderBase.SetDomainNamespace(_domainNamespace?.Value);
            builderBase.SetApplicationNamespace(_applicationNamespace?.Value);
            builderBase.SetInfrastructureNamespace(_infrastructureNamespace?.Value);
            builderBase.SetApiNamespace(_apiNamespace?.Value);

            return builderBase;
        }

        public T WithSettings(Settings options)
        {
            _options = options;
            return this as T;
        }

        public T SetApiDirectory(string apiDirectory)
        {
            _apiDirectory = (Token)apiDirectory;
            return this as T;
        }

        public T SetDirectory(string directory)
        {
            _directory = (Token)directory;
            return this as T;
        }

        public T SetDomainDirectory(string domainDirectory)
        {
            _domainDirectory = (Token)domainDirectory;
            return this as T;
        }

        public T SetRootNamespace(string rootNamespace)
        {
            _rootNamespace = (Token)rootNamespace;
            return this as T;
        }
        public T SetApiNamespace(string apiNamespace)
        {
            _apiNamespace = (Token)apiNamespace;
            return this as T;
        }

        public T SetNamespace(string @namespace)
        {
            _namespace = (Token)@namespace;
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

        public T SetApplicationDirectory(string applicationDirectory)
        {
            _applicationDirectory = (Token)applicationDirectory;
            return this as T;
        }

        public T SetInfrastructureNamespace(string infrastructureNamespace)
        {
            _infrastructureNamespace = (Token)infrastructureNamespace;
            return this as T;
        }

        public T SetInfrastructureDirectory(string infrastructureNamespace)
        {
            _infrastructureDirectory = (Token)infrastructureNamespace;
            return this as T;
        }
    }
}
