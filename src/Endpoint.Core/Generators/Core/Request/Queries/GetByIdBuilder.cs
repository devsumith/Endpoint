
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using Endpoint.Core;
using Endpoint.Core.Builders.Statements;
using Endpoint.Core.Builders.Common;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.Builders
{
    public class GetByIdBuilder
    {
        private readonly List<string> _content;
        private readonly IContext _context;
        private readonly IFileSystem _fileSystem;
        private string _entity;
        private string _dbContext;
        private string _directory;
        private string _namespace;
        private string _domainNamespace;
        private string _applicationNamespace;
        private SettingsModel _settings;

        public GetByIdBuilder(SettingsModel settings, IContext context, IFileSystem fileSystem)
        {
            _content = new();
            _context = context;
            _fileSystem = fileSystem;
            _settings = settings;
        }

        public GetByIdBuilder WithDomainNamespace(string domainNamespace)
        {
            _domainNamespace = domainNamespace;
            return this;
        }

        public GetByIdBuilder WithApplicationNamespace(string applicationNamespace)
        {
            _applicationNamespace = applicationNamespace;
            return this;
        }

        public GetByIdBuilder WithDirectory(string directory)
        {
            _directory = directory;
            return this;
        }

        public GetByIdBuilder WithEntity(string entity)
        {
            _entity = entity;
            return this;
        }

        public GetByIdBuilder WithDbContext(string dbContext)
        {
            _dbContext = dbContext;
            return this;
        }

        public GetByIdBuilder WithNamespace(string @namespace)
        {
            _namespace = @namespace;
            return this;
        }

        public void Build()
        {

            var request = new ClassBuilder($"Get{((Token)_entity).PascalCase}ByIdRequest", _context, _fileSystem)
                .WithInterface(new TypeBuilder().WithGenericType("IRequest", $"Get{((Token)_entity).PascalCase}ByIdResponse").Build())
                .WithProperty(new PropertyBuilder().WithType(IdDotNetTypeBuilder.Build(_settings,_entity)).WithName(IdPropertyNameBuilder.Build(_settings, _entity)).WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var response = new ClassBuilder($"Get{((Token)_entity).PascalCase}ByIdResponse", _context, _fileSystem)
                .WithBase("ResponseBase")
                .WithProperty(new PropertyBuilder().WithType($"{((Token)_entity).PascalCase}Dto").WithName($"{((Token)_entity).PascalCase}").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var handlerBody = _settings.IdDotNetType == IdPropertyType.Int ? new List<string>() {
                "return new () {",
                $"{((Token)_entity).PascalCase} = (await _context.{((Token)_entity).PascalCasePlural}.AsNoTracking().SingleOrDefaultAsync(x => x.{IdPropertyNameBuilder.Build(_settings, _entity)} == request.{IdPropertyNameBuilder.Build(_settings, _entity)})).ToDto()".Indent(1),
                "};"
                }
            : new List<string>() {
                "return new () {",
                $"{((Token)_entity).PascalCase} = (await _context.{((Token)_entity).PascalCasePlural}.AsNoTracking().SingleOrDefaultAsync(x => x.{IdPropertyNameBuilder.Build(_settings, _entity)} == new {((Token)_entity).PascalCase}Id(request.{IdPropertyNameBuilder.Build(_settings, _entity)}))).ToDto()".Indent(1),
                "};"
                };

            var handler = new ClassBuilder($"Get{((Token)_entity).PascalCase}ByIdHandler", _context, _fileSystem)
                .WithBase(new TypeBuilder().WithGenericType("IRequestHandler", $"Get{((Token)_entity).PascalCase}ByIdRequest", $"Get{((Token)_entity).PascalCase}ByIdResponse").Build())
                .WithDependency($"I{((Token)_dbContext).PascalCase}", "context")
                .WithDependency($"ILogger<Get{((Token)_entity).PascalCase}ByIdHandler>", "logger")
                .WithMethod(new MethodBuilder().WithName("Handle").WithAsync(true)
                .WithReturnType(new TypeBuilder().WithGenericType("Task", $"Get{((Token)_entity).PascalCase}ByIdResponse").Build())
                .WithParameter(new ParameterBuilder($"Get{((Token)_entity).PascalCase}ByIdRequest", "request").Build())
                .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build())
                .WithBody(handlerBody).Build())
                .Class;

            new NamespaceBuilder($"Get{((Token)_entity).PascalCase}ById", _context, _fileSystem)
                .WithDirectory(_directory)
                .WithNamespace(_namespace)
                .WithUsing("Microsoft.Extensions.Logging")
                .WithUsing("MediatR")
                .WithUsing("System")
                .WithUsing("System.Threading")
                .WithUsing("System.Threading.Tasks")
                .WithUsing("System.Linq")
                .WithUsing(_domainNamespace)
                .WithUsing($"{_applicationNamespace}.Interfaces")
                .WithUsing("Microsoft.EntityFrameworkCore")
                .WithClass(request)
                .WithClass(response)
                .WithClass(handler)
                .Build();
        }
    }
}
