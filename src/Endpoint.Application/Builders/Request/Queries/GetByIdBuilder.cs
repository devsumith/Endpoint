using Endpoint.Application.Extensions;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Application.Builders
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

        public GetByIdBuilder(IContext context, IFileSystem fileSystem)
        {
            _content = new();
            _context = context;
            _fileSystem = fileSystem;
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
                .WithProperty(new PropertyBuilder().WithType("Guid").WithName($"{((Token)_entity).PascalCase}Id").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var response = new ClassBuilder($"Get{((Token)_entity).PascalCase}ByIdResponse", _context, _fileSystem)
                .WithBase("ResponseBase")
                .WithProperty(new PropertyBuilder().WithType($"{((Token)_entity).PascalCase}Dto").WithName($"{((Token)_entity).PascalCase}").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var handler = new ClassBuilder($"Get{((Token)_entity).PascalCase}ByIdHandler", _context, _fileSystem)
                .WithBase(new TypeBuilder().WithGenericType("IRequestHandler", $"Get{((Token)_entity).PascalCase}ByIdRequest", $"Get{((Token)_entity).PascalCase}ByIdResponse").Build())
                .WithDependency($"I{((Token)_dbContext).PascalCase}", "context")
                .WithDependency($"ILogger<Get{((Token)_entity).PascalCase}ByIdHandler>", "logger")
                .WithMethod(new MethodBuilder().WithName("Handle").WithAsync(true)
                .WithReturnType(new TypeBuilder().WithGenericType("Task", $"Get{((Token)_entity).PascalCase}ByIdResponse").Build())
                .WithParameter(new ParameterBuilder($"Get{((Token)_entity).PascalCase}ByIdRequest", "request").Build())
                .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build())
                .WithBody(new List<string>() {
                "return new () {",
                $"{((Token)_entity).PascalCase} = (await _context.{((Token)_entity).PascalCasePlural}.SingleOrDefaultAsync(x => x.{((Token)_entity).PascalCase}Id == request.{((Token)_entity).PascalCase}Id)).ToDto()".Indent(1),
                "};"
                }).Build())
                .Class;

            new NamespaceBuilder($"Get{((Token)_entity).PascalCase}ById", _context, _fileSystem)
                .WithDirectory(_directory)
                .WithNamespace(_namespace)
                .WithUsing("Microsoft.Extensions.Logging")
                .WithUsing("MediatR")
                .WithUsing("System")
                .WithUsing("System.Threading")
                .WithUsing("System.Threading.Tasks")
                .WithUsing($"{_domainNamespace}.Core")
                .WithUsing($"{_applicationNamespace}.Interfaces")
                .WithUsing("Microsoft.EntityFrameworkCore")
                .WithClass(request)
                .WithClass(response)
                .WithClass(handler)
                .Build();
        }
    }
}
