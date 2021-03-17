using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System.IO;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.Application.Builders
{
    public class ResourceBuilder : BuilderBase<ResourceBuilder>
    {
        public ResourceBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        private string _entityName;
        private string _dbContextName;

        public ResourceBuilder WithResource(string resource)
        {
            _entityName = resource;
            return this;
        }

        public ResourceBuilder WithDbContext(string dbContext)
        {
            _dbContextName = dbContext;
            return this;
        }

        public void Build()
        {
            new ModelBuilder(
                new Context(), 
                _fileSystem, 
                _domainDirectory.Value, 
                _domainNamespace.Value, 
                _entityName
                ).Build();

            new DbContextBuilder(
                _dbContextName,
                new Context(),
                _fileSystem,
                _infrastructureDirectory.Value,
                _infrastructureNamespace.Value,
                _domainDirectory.Value,
                _domainNamespace.Value,
                _applicationDirectory.Value,
                _applicationNamespace.Value,
                new string[1] { _entityName }).Build();
            Map(Create<FeatureBuilder>((a, b, c, d) => new(a, b, c, d)))
                .SetEntityName(_entityName)
                .Build();

            Map(Create<ControllerBuilder>((a, b, c, d) => new(a, b, c, d)))
                .SetResource(_entityName)
                .SetApiDirectory(_apiDirectory.Value)
                .SetApiNamespace(_apiNamespace.Value)
                .Build();
        }
    }
}
