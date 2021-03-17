using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class FeatureBuilder : BuilderBase<FeatureBuilder>
    {
        private Token _entityName;
        private bool _eventSourcing;
        private readonly IContext _context;
        public FeatureBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        {
            _context = new Context();
        }

        public FeatureBuilder(
            IContext context,
            IFileSystem fileSystem,
            string domainDirectory,
            string domainNamespace,
            string entityName
            ) : base(default, default, default, fileSystem)
        {
            _context = context;

            _domainDirectory = (Token)domainDirectory;
            _domainNamespace = (Token)domainNamespace;
            _entityName = (Token)entityName;
        }

        public FeatureBuilder SetEntityName(string entityName)
        {
            _entityName = (Token)entityName;
            return this;
        }

        public FeatureBuilder SetEventSourcing(bool eventSourcing)
        {
            _eventSourcing = eventSourcing;
            return this;
        }

        public void Build()
        {
            
            var featureFolder = $"{_applicationDirectory.Value}{Path.DirectorySeparatorChar}Features{Path.DirectorySeparatorChar}{_entityName.PascalCasePlural}";

            if(!Directory.Exists(featureFolder))
            {
                _commandService.Start($"mkdir {featureFolder}");

            }
            new ClassBuilder($"{_entityName.PascalCase}Dto", _context, _fileSystem)
                .WithDirectory(featureFolder)
                .WithUsing("System")
                .WithNamespace($"{_applicationNamespace.Value}.Features")
                .WithProperty(new PropertyBuilder().WithName($"{_entityName.PascalCase}Id").WithType("Guid").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Build();
        }
    }
}
