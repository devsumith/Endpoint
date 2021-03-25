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
        private string _dbContext;
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

        public FeatureBuilder WithDbContext(string dbContext)
        {
            _dbContext = dbContext;
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

            new ClassBuilder($"{((Token)_entityName).PascalCase}Extensions", _context, _fileSystem)
                .WithDirectory($"{_applicationDirectory.Value}{Path.DirectorySeparatorChar}Features{Path.DirectorySeparatorChar}{((Token)_entityName).PascalCasePlural}")
                .WithUsing("System")
                .IsStatic()
                .WithUsing($"{_domainNamespace.Value}.Models")
                .WithNamespace($"{_applicationNamespace.Value}.Features")
                .WithMethod(new MethodBuilder()
                .IsStatic()
                .WithName("ToDto")
                .WithReturnType($"{((Token)_entityName).PascalCase}Dto")
                .WithPropertyName($"{((Token)_entityName).PascalCase}Id")
                .WithParameter(new ParameterBuilder(((Token)_entityName).PascalCase, ((Token)_entityName).CamelCase, true).Build())
                .WithBody(new()
                {
                    "    return new ()",
                    "    {",
                    $"        {((Token)_entityName).PascalCase}Id = {((Token)_entityName).CamelCase}.{((Token)_entityName).PascalCase}Id",
                    "    };"
                })
                .Build())
                .Build();

            new ClassBuilder($"{_entityName.PascalCase}Validator", new Context(), _fileSystem)
                .WithDirectory($"{_applicationDirectory.Value}{Path.DirectorySeparatorChar}Features{Path.DirectorySeparatorChar}{((Token)_entityName).PascalCasePlural}")
                .WithBase(new TypeBuilder().WithGenericType("AbstractValidator", $"{_entityName.PascalCase}Dto").Build())
                .WithNamespace($"{_applicationNamespace.Value}.Features")
                .WithUsing("FluentValidation")
                .Build();

            new CreateBuilder(new Context(), _fileSystem)
                .WithDirectory($"{_applicationDirectory.Value}{Path.DirectorySeparatorChar}Features{Path.DirectorySeparatorChar}{((Token)_entityName).PascalCasePlural}")
                .WithDbContext(_dbContext)
                .WithNamespace($"{_applicationNamespace.Value}.Features")
                .WithApplicationNamespace($"{_applicationNamespace.Value}")
                .WithDomainNamespace($"{_domainNamespace.Value}")
                .WithEntity(_entityName.Value)
                .Build();

            Map(BuilderFactory.Create<QueryBuilder>((a, b, c, d) => new(a, b, c, d)))
                .SetDirectory($"{_applicationDirectory.Value}{Path.DirectorySeparatorChar}Features{Path.DirectorySeparatorChar}{((Token)_entityName).PascalCasePlural}")
                .SetApplicationNamespace($"{_applicationNamespace.Value}")
                .SetDomainNamespace(_domainNamespace.Value)
                .WithEntity(_entityName)
                .WithName($"Get{_entityName.PascalCasePlural}")
                .WithDbContext(_dbContext)
                .Build();

            Map(BuilderFactory.Create<QueryBuilder>((a, b, c, d) => new(a, b, c, d)))
                .SetDirectory($"{_applicationDirectory.Value}{Path.DirectorySeparatorChar}Features{Path.DirectorySeparatorChar}{((Token)_entityName).PascalCasePlural}")
                .SetApplicationNamespace($"{_applicationNamespace.Value}")
                .SetDomainNamespace(_domainNamespace.Value)
                .WithEntity(_entityName)
                .WithName($"Get{_entityName.PascalCase}ById")
                .WithDbContext(_dbContext)
                .Build();
        }
    }
}
