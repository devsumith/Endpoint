using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class ModelBuilder : BuilderBase<ModelBuilder>
    {
        private Token _entityName;
        private bool _eventSourcing;
        private readonly IContext _context;
        public ModelBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public ModelBuilder(
            IContext context,
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        {
            _context = context;
        }

        public ModelBuilder(
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

        public ModelBuilder SetEntityName(string entityName)
        {
            _entityName = (Token)entityName;
            return this;
        }

        public ModelBuilder SetEventSourcing(bool eventSourcing)
        {
            _eventSourcing = eventSourcing;
            return this;
        }

        public void Build()
        {
            new ClassBuilder(_entityName.PascalCase, _context, _fileSystem)
                .WithDirectory($"{_domainDirectory.Value}{Path.DirectorySeparatorChar}Models")
                .WithUsing("System")
                .WithNamespace($"{_domainNamespace.Value}.Models")
                .WithProperty(new PropertyBuilder().WithName($"{_entityName.PascalCase}Id").WithType("Guid").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Build();
        }
    }
}
