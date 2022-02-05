
using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
using System.Collections.Generic;
using Endpoint.SharedKernal;

namespace Endpoint.Application.Builders
{
    public class CreateBuilder
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

        public CreateBuilder(IContext context, IFileSystem fileSystem)
        {
            _content = new();
            _context = context;
            _fileSystem = fileSystem;
        }

        public CreateBuilder WithDomainNamespace(string domainNamespace)
        {
            _domainNamespace = domainNamespace;
            return this;
        }

        public CreateBuilder WithApplicationNamespace(string applicationNamespace)
        {
            _applicationNamespace = applicationNamespace;
            return this;
        }

        public CreateBuilder WithDirectory(string directory)
        {
            _directory = directory;
            return this;
        }

        public CreateBuilder WithEntity(string entity)
        {
            _entity = entity;
            return this;
        }

        public CreateBuilder WithDbContext(string dbContext)
        {
            _dbContext = dbContext;
            return this;
        }


        public CreateBuilder WithNamespace(string @namespace)
        {
            _namespace = @namespace;
            return this;
        }

        public void Build()
        {

            var validator = new ClassBuilder($"Create{((Token)_entity).PascalCase}Validator", _context, _fileSystem)
                .WithRuleFor(new RuleForBuilder().WithNotNull().WithEntity(_entity).Build())
                .WithRuleFor(new RuleForBuilder().WithValidator().WithEntity(_entity).Build())
                .WithBase(new TypeBuilder().WithGenericType("AbstractValidator", $"Create{((Token)_entity).PascalCase}Request").Build())
                .Class;

            var request = new ClassBuilder($"Create{((Token)_entity).PascalCase}Request", _context, _fileSystem)
                .WithInterface(new TypeBuilder().WithGenericType("IRequest", $"Create{((Token)_entity).PascalCase}Response").Build())
                .WithProperty(new PropertyBuilder().WithType($"{((Token)_entity).PascalCase}Dto").WithName($"{((Token)_entity).PascalCase}").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var response = new ClassBuilder($"Create{((Token)_entity).PascalCase}Response", _context, _fileSystem)
                .WithBase("ResponseBase")
                .WithProperty(new PropertyBuilder().WithType($"{((Token)_entity).PascalCase}Dto").WithName($"{((Token)_entity).PascalCase}").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var handler = new ClassBuilder($"Create{((Token)_entity).PascalCase}Handler", _context, _fileSystem)
                .WithBase(new TypeBuilder().WithGenericType("IRequestHandler", $"Create{((Token)_entity).PascalCase}Request", $"Create{((Token)_entity).PascalCase}Response").Build())
                .WithDependency($"I{((Token)_dbContext).PascalCase}", "context")
                .WithDependency($"ILogger<Create{((Token)_entity).PascalCase}Handler>", "logger")
                .WithMethod(new MethodBuilder().WithName("Handle").WithAsync(true)
                .WithReturnType(new TypeBuilder().WithGenericType("Task", $"Create{((Token)_entity).PascalCase}Response").Build())
                .WithParameter(new ParameterBuilder($"Create{((Token)_entity).PascalCase}Request", "request").Build())
                .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build())
                .WithBody(new List<string>() {
                $"var {((Token)_entity).CamelCase} = new {((Token)_entity).PascalCase}();",
                "",
                $"_context.{((Token)_entity).PascalCasePlural}.Add({((Token)_entity).CamelCase});",
                "",
                "await _context.SaveChangesAsync(cancellationToken);",
                "",
                "return new ()",
                "{",
                $"{((Token)_entity).PascalCase} = {((Token)_entity).CamelCase}.ToDto()".Indent(1),
                "};"
                }).Build())
                .Class;

            new NamespaceBuilder($"Create{((Token)_entity).PascalCase}", _context, _fileSystem)
                .WithDirectory(_directory)
                .WithNamespace(_namespace)
                .WithUsing("FluentValidation")
                .WithUsing("Microsoft.Extensions.Logging")
                .WithUsing("MediatR")
                .WithUsing("System")
                .WithUsing("System.Threading")
                .WithUsing("System.Threading.Tasks")
                .WithUsing("System.Linq")
                .WithUsing(_domainNamespace)
                .WithUsing($"{_applicationNamespace}.Interfaces")
                .WithClass(validator)
                .WithClass(request)
                .WithClass(response)
                .WithClass(handler)
                .Build();
        }
    }
}
