// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Services;
using System.Collections.Generic;
using Endpoint.Core;
using Endpoint.Core.Builders.Core;
using System.Linq;
using Endpoint.Core.Models.Syntax.Entities.Legacy;
using Endpoint.Core.Models.Syntax;

namespace Endpoint.Core.Builders
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
        private LegacyAggregateModel _aggregateRoot;

        public CreateBuilder(IContext context, IFileSystem fileSystem)
        {
            _content = new();
            _context = context;
            _fileSystem = fileSystem;
        }

        public CreateBuilder WithAggregateRoot(LegacyAggregateModel aggregateRoot)
        {
            _aggregateRoot = aggregateRoot;
            return this;
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

            var validator = new ClassBuilder($"Create{((SyntaxToken)_entity).PascalCase}Validator", _context, _fileSystem)
                .WithRuleFor(new RuleForBuilder().WithNotNull().WithEntity(_entity).Build())
                .WithRuleFor(new RuleForBuilder().WithValidator().WithEntity(_entity).Build())
                .WithBase(new TypeBuilder().WithGenericType("AbstractValidator", $"Create{((SyntaxToken)_entity).PascalCase}Request").Build())
                .Class;

            var request = new ClassBuilder($"Create{((SyntaxToken)_entity).PascalCase}Request", _context, _fileSystem)
                .WithInterface(new TypeBuilder().WithGenericType("IRequest", $"Create{((SyntaxToken)_entity).PascalCase}Response").Build())
                .WithProperty(new PropertyBuilder().WithType($"{((SyntaxToken)_entity).PascalCase}Dto").WithName($"{((SyntaxToken)_entity).PascalCase}").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var response = new ClassBuilder($"Create{((SyntaxToken)_entity).PascalCase}Response", _context, _fileSystem)
                .WithBase("ResponseBase")
                .WithProperty(new PropertyBuilder().WithType($"{((SyntaxToken)_entity).PascalCase}Dto").WithName($"{((SyntaxToken)_entity).PascalCase}").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var handler = new ClassBuilder($"Create{((SyntaxToken)_entity).PascalCase}Handler", _context, _fileSystem)
                .WithBase(new TypeBuilder().WithGenericType("IRequestHandler", $"Create{((SyntaxToken)_entity).PascalCase}Request", $"Create{((SyntaxToken)_entity).PascalCase}Response").Build())
                .WithDependency($"I{((SyntaxToken)_dbContext).PascalCase}", "context")
                .WithDependency($"ILogger<Create{((SyntaxToken)_entity).PascalCase}Handler>", "logger")
                .WithMethod(new MethodBuilder().WithName("Handle").WithAsync(true)
                .WithReturnType(new TypeBuilder().WithGenericType("Task", $"Create{((SyntaxToken)_entity).PascalCase}Response").Build())
                .WithParameter(new ParameterBuilder($"Create{((SyntaxToken)_entity).PascalCase}Request", "request").Build())
                .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build())
                .WithBody(CreateCommandHandlerBodyBuilder.Build(_aggregateRoot).ToList()).Build())
                .Class;

            new NamespaceBuilder($"Create{((SyntaxToken)_entity).PascalCase}", _context, _fileSystem)
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

