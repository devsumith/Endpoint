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
            Map(Create<ModelBuilder>((a, b, c, d) => new(a, b, c, d)))
                .SetEntityName(_entityName)
                .Build();

            Map(Create<DbContextBuilder>((a, b, c, d) => new(a, b, c, d)))
                .SetDirectory($@"{_domainDirectory}{Path.DirectorySeparatorChar}{Constants.Folders.Data}")
                .SetNamespace($"{_domainNamespace}.{Constants.Folders.Data}")
                .SetModelsDirectory($"{_domainDirectory}{Path.DirectorySeparatorChar}Models")
                .WithModel(_entityName)
                .WithDbContextName(_dbContextName)
                .Build();
        }
    }
}
