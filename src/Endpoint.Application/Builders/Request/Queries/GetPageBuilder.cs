using Endpoint.Application.Extensions;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Application.Builders
{
    public class GetPageBuilder
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

        public GetPageBuilder(IContext context, IFileSystem fileSystem)
        {
            _content = new();
            _context = context;
            _fileSystem = fileSystem;
        }

        public GetPageBuilder WithDomainNamespace(string domainNamespace)
        {
            _domainNamespace = domainNamespace;
            return this;
        }

        public GetPageBuilder WithApplicationNamespace(string applicationNamespace)
        {
            _applicationNamespace = applicationNamespace;
            return this;
        }

        public GetPageBuilder WithDirectory(string directory)
        {
            _directory = directory;
            return this;
        }

        public GetPageBuilder WithEntity(string entity)
        {
            _entity = entity;
            return this;
        }

        public GetPageBuilder WithDbContext(string dbContext)
        {
            _dbContext = dbContext;
            return this;
        }

        public GetPageBuilder WithNamespace(string @namespace)
        {
            _namespace = @namespace;
            return this;
        }

        public void Build()
        {
            
            var request = new ClassBuilder("Request", _context, _fileSystem)
                .WithInterface(new TypeBuilder().WithGenericType("IRequest", "Response").Build())
                .WithProperty(new PropertyBuilder().WithType("int").WithName("PageSize").WithAccessors(new AccessorsBuilder().Build()).Build())
                .WithProperty(new PropertyBuilder().WithType("int").WithName("Index").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var response = new ClassBuilder("Response", _context, _fileSystem)
                .WithBase("ResponseBase")
                .WithProperty(new PropertyBuilder().WithType("int").WithName("Length").WithAccessors(new AccessorsBuilder().Build()).Build())
                .WithProperty(new PropertyBuilder().WithType(new TypeBuilder().WithGenericType("List", $"{((Token)_entity).PascalCase}Dto").Build()).WithName($"Entities").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var handler = new ClassBuilder("Handler", _context, _fileSystem)
                .WithBase(new TypeBuilder().WithGenericType("IRequestHandler", "Request", "Response").Build())
                .WithDependency($"I{((Token)_dbContext).PascalCase}", "context")
                .WithMethod(new MethodBuilder().WithName("Handle").WithAsync(true)
                .WithReturnType(new TypeBuilder().WithGenericType("Task", "Response").Build())
                .WithParameter(new ParameterBuilder("Request", "request").Build())
                .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build())
                .WithBody(new List<string>() {
                $"var query = from {((Token)_entity).CamelCase} in _context.{((Token)_entity).PascalCasePlural}",
                $"select {((Token)_entity).CamelCase};".Indent(1),
                "",
                $"var length = await _context.{((Token)_entity).PascalCasePlural}.CountAsync();",
                "",
                $"var {((Token)_entity).CamelCasePlural} = await query.Page(request.Index, request.PageSize)",
                ".Select(x => x.ToDto()).ToListAsync();".Indent(1),
                "",
                "return new()",
                "{",
                "Length = length,".Indent(1),
                $"Entities = {((Token)_entity).CamelCasePlural}".Indent(1),
                "};"
                }).Build())
                .Class;

            new ClassBuilder($"Get{((Token)_entity).PascalCasePlural}Page", _context, _fileSystem)
                .WithDirectory(_directory)
                .WithNamespace(_namespace)
                .WithUsing("MediatR")
                .WithUsing("System")
                .WithUsing("System.Threading")
                .WithUsing("System.Threading.Tasks")
                .WithUsing("System.Collections.Generic")
                .WithUsing("System.Linq")
                .WithUsing($"{_applicationNamespace}.Extensions")
                .WithUsing($"{_domainNamespace}.Core")
                .WithUsing($"{_applicationNamespace}.Interfaces")
                .WithUsing($"{_domainNamespace}.Extensions")
                .WithUsing("Microsoft.EntityFrameworkCore")
                .WithClass(request)
                .WithClass(response)
                .WithClass(handler)
                .Build();
        }
    }
}
