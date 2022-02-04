using Endpoint.Application.Extensions;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Application.Builders
{
    public class GetBuilder
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

        public GetBuilder(IContext context, IFileSystem fileSystem)
        {
            _content = new();
            _context = context;
            _fileSystem = fileSystem;
        }

        public GetBuilder WithDomainNamespace(string domainNamespace)
        {
            _domainNamespace = domainNamespace;
            return this;
        }

        public GetBuilder WithApplicationNamespace(string applicationNamespace)
        {
            _applicationNamespace = applicationNamespace;
            return this;
        }

        public GetBuilder WithDirectory(string directory)
        {
            _directory = directory;
            return this;
        }

        public GetBuilder WithEntity(string entity)
        {
            _entity = entity;
            return this;
        }

        public GetBuilder WithDbContext(string dbContext)
        {
            _dbContext = dbContext;
            return this;
        }

        public GetBuilder WithNamespace(string @namespace)
        {
            _namespace = @namespace;
            return this;
        }

        public void Build()
        {

            var request = new ClassBuilder($"Get{((Token)_entity).PascalCasePlural}Request", _context, _fileSystem)
                .WithInterface(new TypeBuilder().WithGenericType("IRequest", $"Get{((Token)_entity).PascalCasePlural}Response").Build())
                .Class;

            var response = new ClassBuilder($"Get{((Token)_entity).PascalCasePlural}Response", _context, _fileSystem)
                .WithBase("ResponseBase")
                .WithProperty(new PropertyBuilder().WithType(new TypeBuilder().WithGenericType("List", $"{ ((Token)_entity).PascalCase}Dto").Build()).WithName($"{((Token)_entity).PascalCasePlural}").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var handler = new ClassBuilder($"Get{((Token)_entity).PascalCasePlural}Handler", _context, _fileSystem)
                .WithBase(new TypeBuilder().WithGenericType("IRequestHandler", $"Get{((Token)_entity).PascalCasePlural}Request", $"Get{((Token)_entity).PascalCasePlural}Response").Build())
                .WithDependency($"I{((Token)_dbContext).PascalCase}", "context")
                .WithDependency($"ILogger<Get{((Token)_entity).PascalCasePlural}Handler>", "logger")
                .WithMethod(new MethodBuilder().WithName("Handle").WithAsync(true)
                .WithReturnType(new TypeBuilder().WithGenericType("Task", $"Get{((Token)_entity).PascalCasePlural}Response").Build())
                .WithParameter(new ParameterBuilder($"Get{((Token)_entity).PascalCasePlural}Request", "request").Build())
                .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build())
                .WithBody(new List<string>() {
                "return new () {",
                $"{((Token)_entity).PascalCasePlural} = await _context.{((Token)_entity).PascalCasePlural}.AsNoTracking().ToDtosAsync(cancellationToken)".Indent(1),
                "};"
                }).Build())
                .Class;

            new NamespaceBuilder($"Get{((Token)_entity).PascalCasePlural}", _context, _fileSystem)
                .WithDirectory(_directory)
                .WithNamespace(_namespace)
                .WithUsing("Microsoft.Extensions.Logging")
                .WithUsing("MediatR")
                .WithUsing("System")
                .WithUsing("System.Threading")
                .WithUsing("System.Threading.Tasks")
                .WithUsing("System.Linq")
                .WithUsing("System.Collections.Generic")
                .WithUsing("System.Linq")
                .WithUsing(_domainNamespace)
                .WithUsing($"{_applicationNamespace}.Interfaces")
                .WithUsing("Microsoft.EntityFrameworkCore")
                .WithUsing("Microsoft.Extensions.Logging")
                .WithClass(request)
                .WithClass(response)
                .WithClass(handler)
                .Build();
        }
    }
}
