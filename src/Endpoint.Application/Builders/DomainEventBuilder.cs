using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System;
using System.IO;

namespace Endpoint.Application.Builders
{
    public class DomainEventBuilder : BuilderBase<DomainEventBuilder>
    {
        private Token _entityName;
        private bool _eventSourcing;
        private readonly IContext _context;
        public DomainEventBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public DomainEventBuilder(
            IContext context,
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        {
            _context = context;
        }

        public DomainEventBuilder(
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

        public DomainEventBuilder SetEntityName(string entityName)
        {
            _entityName = (Token)entityName;
            return this;
        }

        public void Build()
        {
            new ClassBuilder(_entityName.PascalCase, _context, _fileSystem)
                .WithDirectory($"{_domainDirectory.Value}{Path.DirectorySeparatorChar}DomainEvents")
                .WithUsing("System")
                .WithNamespace($"{_domainNamespace.Value}.DomainEvents")
                .Build();
        }
    }
}
