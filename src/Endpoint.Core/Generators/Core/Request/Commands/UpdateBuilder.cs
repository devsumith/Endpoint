// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.


using Endpoint.Core.Services;
using System.Collections.Generic;
using Endpoint.Core;
using Endpoint.Core.Builders.Core;
using System.Linq;
using Endpoint.Core.Syntax.Entities.Legacy;
using Endpoint.Core.Syntax;
using Endpoint.Core.Options;

namespace Endpoint.Core.Builders
{
    public class UpdateBuilder
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
        private LegacyAggregatesModel _aggregateRoot;
        private SettingsModel _settings;

        public UpdateBuilder(SettingsModel settings, IContext context, IFileSystem fileSystem)
        {
            _content = new();
            _context = context;
            _fileSystem = fileSystem;
            _settings = settings;
        }

        public UpdateBuilder WithAggregateRoot(LegacyAggregatesModel aggregateRoot)
        {
            _aggregateRoot = aggregateRoot;
            return this;
        }
        public UpdateBuilder WithDomainNamespace(string domainNamespace)
        {
            _domainNamespace = domainNamespace;
            return this;
        }

        public UpdateBuilder WithApplicationNamespace(string applicationNamespace)
        {
            _applicationNamespace = applicationNamespace;
            return this;
        }

        public UpdateBuilder WithDirectory(string directory)
        {
            _directory = directory;
            return this;
        }

        public UpdateBuilder WithEntity(string entity)
        {
            _entity = entity;
            return this;
        }

        public UpdateBuilder WithDbContext(string dbContext)
        {
            _dbContext = dbContext;
            return this;
        }


        public UpdateBuilder WithNamespace(string @namespace)
        {
            _namespace = @namespace;
            return this;
        }

        public void Build()
        {

            var validator = new ClassBuilder($"Update{((SyntaxToken)_entity).PascalCase}Validator", _context, _fileSystem)
                .WithRuleFor(new RuleForBuilder().WithNotNull().WithEntity(_entity).Build())
                .WithRuleFor(new RuleForBuilder().WithValidator().WithEntity(_entity).Build())
                .WithBase(new TypeBuilder().WithGenericType("AbstractValidator", $"Update{((SyntaxToken)_entity).PascalCase}Request").Build())
                .Class;

            var request = new ClassBuilder($"Update{((SyntaxToken)_entity).PascalCase}Request", _context, _fileSystem)
                .WithInterface(new TypeBuilder().WithGenericType("IRequest", $"Update{((SyntaxToken)_entity).PascalCase}Response").Build())
                .WithProperty(new PropertyBuilder().WithType($"{((SyntaxToken)_entity).PascalCase}Dto").WithName($"{((SyntaxToken)_entity).PascalCase}").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var response = new ClassBuilder($"Update{((SyntaxToken)_entity).PascalCase}Response", _context, _fileSystem)
                .WithBase("ResponseBase")
                .WithProperty(new PropertyBuilder().WithType($"{((SyntaxToken)_entity).PascalCase}Dto").WithName($"{((SyntaxToken)_entity).PascalCase}").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Class;

            var handler = new ClassBuilder($"Update{((SyntaxToken)_entity).PascalCase}Handler", _context, _fileSystem)
                .WithBase(new TypeBuilder().WithGenericType("IRequestHandler", $"Update{((SyntaxToken)_entity).PascalCase}Request", $"Update{((SyntaxToken)_entity).PascalCase}Response").Build())
                .WithDependency($"I{((SyntaxToken)_dbContext).PascalCase}", "context")
                .WithDependency($"ILogger<Update{((SyntaxToken)_entity).PascalCase}Handler>", "logger")
                .WithMethod(new MethodBuilder().WithName("Handle").WithAsync(true)
                .WithReturnType(new TypeBuilder().WithGenericType("Task", $"Update{((SyntaxToken)_entity).PascalCase}Response").Build())
                .WithParameter(new ParameterBuilder($"Update{((SyntaxToken)_entity).PascalCase}Request", "request").Build())
                .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build())
                .WithBody(UpdateCommandHandlerBodyBuilder.Build(_settings, _aggregateRoot).ToList()).Build())
                .Class;

            new NamespaceBuilder($"Update{((SyntaxToken)_entity).PascalCase}", _context, _fileSystem)
                .WithDirectory(_directory)
                .WithNamespace(_namespace)
                .WithUsing("Microsoft.Extensions.Logging")
                .WithUsing("FluentValidation")
                .WithUsing("MediatR")
                .WithUsing("System")
                .WithUsing("System.Threading")
                .WithUsing("System.Threading.Tasks")
                .WithUsing("System.Linq")
                .WithUsing(_domainNamespace)
                .WithUsing($"{_applicationNamespace}.Interfaces")
                .WithUsing("Microsoft.EntityFrameworkCore")
                .WithClass(validator)
                .WithClass(request)
                .WithClass(response)
                .WithClass(handler)
                .Build();
        }
    }
}

