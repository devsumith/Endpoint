/*using Endpoint.Application.Enums;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System.Collections.Generic;
using System.IO;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.Application.Builders
{
    public class ResourceBuilder
    {
        private readonly Models.Settings _settings;
        protected readonly ICommandService _commandService;
        protected readonly ITemplateProcessor _templateProcessor;
        protected readonly ITemplateLocator _templateLocator;
        protected readonly IFileSystem _fileSystem;
        protected Token _directory;
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
        private string _entityName;
        private string _dbContextName;
        private List<string> _resources = new List<string>();

        public ResourceBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem,
            Models.Settings settings)
        { 
            _settings = settings;
            _dbContextName = settings.DbContextName;
            _domainDirectory = (Token)settings.DomainDirectory;
            _applicationDirectory = (Token)settings.ApplicationDirectory;
            _infrastructureDirectory = (Token)settings.InfrastructureDirectory;
            _apiDirectory = (Token)settings.ApiDirectory;
            _domainNamespace = (Token)settings.DomainNamespace;
            _applicationNamespace = (Token)settings.ApplicationNamespace;
            _infrastructureNamespace = (Token)settings.InfrastructureNamespace;
            _apiNamespace = (Token)settings.ApiNamespace;
            _resources = settings.Resources;
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
                _resources.ToArray()).Build();

            Map(Create<FeatureBuilder>((a, b, c, d) => new(a, b, c, d)))
                .SetEntityName(_entityName)
                .WithDbContext(_dbContextName)
                .Build();


            new ClassBuilder($"{((Token)_entityName).PascalCase}Controller", new Context(), _fileSystem)
                .WithDirectory($"{_apiDirectory.Value}{Path.DirectorySeparatorChar}Controllers")                
                .WithUsing("System.Net")
                .WithUsing("System.Threading")
                .WithUsing("System.Threading.Tasks")
                .WithUsing($"{_applicationNamespace.Value}.Features")
                .WithUsing("MediatR")
                .WithUsing("System")
                .WithUsing("Microsoft.AspNetCore.Mvc")
                .WithUsing("Microsoft.Extensions.Logging")
                .WithUsing("Swashbuckle.AspNetCore.Annotations")
                .WithUsing("System.Net.Mime")
                .WithNamespace($"{_apiNamespace.Value}.Controllers")
                .WithAttribute(new AttributeBuilder().WithName("ApiController").Build())
                .WithAttribute(new AttributeBuilder().WithName("Route").WithParam("\"api/[controller]\"").Build())
                .WithAttribute(new AttributeBuilder().WithName("Produces").WithParam("MediaTypeNames.Application.Json").Build())
                .WithAttribute(new AttributeBuilder().WithName("Consumes").WithParam("MediaTypeNames.Application.Json").Build())
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
*/