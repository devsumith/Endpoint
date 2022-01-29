using Endpoint.Application.Enums;
using Endpoint.Application.Models;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System;
using System.IO;
using System.Linq;
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
        private string[] _resources = new string[0];

        public ResourceBuilder WithResource(string resource)
        {
            _entityName = resource;
            _resources = _resources.Concat(new string[1] { resource }).ToArray();
            return this;
        }

        public ResourceBuilder WithResources(string[] resources)
        {
            _resources = resources;
            return this;
        }

        public ResourceBuilder WithDbContext(string dbContext)
        {
            _dbContextName = dbContext;
            return this;
        }

        public new ResourceBuilder WithSettings(Settings settings)
        {
            _dbContextName = settings.DbContext;
            _domainDirectory = (Token)settings.DomainDirectory;
            _applicationDirectory = (Token)settings.ApplicationDirectory;
            _infrastructureDirectory = (Token)settings.InfrastructureDirectory;
            _apiDirectory = (Token)settings.ApiDirectory;
            _domainNamespace = (Token)settings.DomainNamespace;
            _applicationNamespace = (Token)settings.ApplicationNamespace;
            _infrastructureNamespace = (Token)settings.InfrastructureNamespace;
            _apiNamespace = (Token)settings.ApiNamespace;
            _resources = settings.Resources;
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
                _resources).Build();

            Map(Create<FeatureBuilder>((a, b, c, d) => new(a, b, c, d)))
                .SetEntityName(_entityName)
                .WithDbContext(_dbContextName)
                .Build();


            new ClassBuilder($"{((Token)_entityName).PascalCase}Controller", new Context(), _fileSystem)
                .WithDirectory($"{_apiDirectory.Value}{Path.DirectorySeparatorChar}Controllers")
                .WithUsing("System.Net")
                .WithUsing("System.Threading.Tasks")
                .WithUsing($"{_applicationNamespace.Value}.Features")
                .WithUsing("MediatR")
                .WithUsing("System")
                .WithUsing("Microsoft.AspNetCore.Mvc")
                .WithUsing("Microsoft.Extensions.Logging")
                .WithNamespace($"{_apiNamespace.Value}.Controllers")
                .WithAttribute(new AttributeBuilder().WithName("ApiController").Build())
                .WithAttribute(new AttributeBuilder().WithName("Route").WithParam("\"api/[controller]\"").Build())
                .WithDependency("IMediator", "mediator")
                .WithDependency($"ILogger<{((Token)_entityName).PascalCase}Controller>", "logger")
                .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.GetById).WithResource(_entityName).WithAuthorize(false).Build())
                .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Get).WithResource(_entityName).WithAuthorize(false).Build())
                .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Create).WithResource(_entityName).WithAuthorize(false).Build())
                .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Page).WithResource(_entityName).WithAuthorize(false).Build())
                .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Update).WithResource(_entityName).WithAuthorize(false).Build())
                .WithMethod(new MethodBuilder().WithEndpointType(EndpointType.Delete).WithResource(_entityName).WithAuthorize(false).Build())
                .Build();

        }
    }
}
