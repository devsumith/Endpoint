using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using System;
using System.Collections.Generic;

namespace Endpoint.Application.Builders
{
    public class CommandBuilder : BuilderBase<CommandBuilder>
    {
        private Token _name;
        private Token _entityName;
        private Token _dbContext;
        private Token _buildingBlocksCoreNamespace;
        private Token _buildingBlocksEventStoreNamespace;
        private Token _store;
        private Context _context = new Context();

        public CommandBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public CommandBuilder WithBuildingBlocksCoreNamespace(string buildingBlocksCoreNamespace)
        {
            _buildingBlocksCoreNamespace = (Token)buildingBlocksCoreNamespace;
            return this;
        }

        public CommandBuilder WithBuildingBlocksEventStoreNamespace(string buildingBlocksEventStoreNamespace)
        {
            _buildingBlocksEventStoreNamespace = (Token)buildingBlocksEventStoreNamespace;
            return this;
        }

        public CommandBuilder WithDbContext(string dbContext)
        {
            _dbContext = (Token)dbContext;
            return this;
        }

        public CommandBuilder WithEntity(string entity)
        {
            _entityName = (Token)entity;
            return this;
        }

        public CommandBuilder WithName(string name)
        {
            _name = (Token)name;
            return this;
        }

        public CommandBuilder WithStore(string store)
        {
            _store = (Token)store;
            return this;
        }

        public void Build()
        {

            var validator = new ClassBuilder($"{_name.PascalCase}Validator", _context, _fileSystem)
                .WithBase(new TypeBuilder().WithGenericType("AbstractValidator", $"{_name.PascalCase}Request").Build())
                .Class;

            var request = new ClassBuilder($"{_name.PascalCase}Request", _context, _fileSystem)
                .WithInterface(new TypeBuilder().WithGenericType("IRequest", $"{_name.PascalCase}Response").Build())
                .Class;

            var response = new ClassBuilder($"{_name.PascalCase}Response", _context, _fileSystem)
                .WithBase("ResponseBase")
                .Class;

            var handler = new ClassBuilder($"{_name.PascalCase}Handler", _context, _fileSystem)
                .WithBase(new TypeBuilder().WithGenericType("IRequestHandler", $"{_name.PascalCase}Request", $"{_name.PascalCase}Response").Build())
                .WithDependency($"I{((Token)_dbContext).PascalCase}", "context")
                .WithDependency($"ILogger<{_name.PascalCase}Handler>", "logger")
                .WithMethod(new MethodBuilder().WithName("Handle").WithAsync(true)
                .WithReturnType(new TypeBuilder().WithGenericType("Task", $"{_name.PascalCase}Response").Build())
                .WithParameter(new ParameterBuilder($"{_name.PascalCase}Request", "request").Build())
                .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build())
                .WithBody(new List<string>() {
            "return new ()",
            "{",
            "};"
                }).Build())
                .Class;

            new NamespaceBuilder($"{_name.PascalCase}", _context, _fileSystem)
                .WithDirectory(_directory)
                .WithNamespace(_namespace)
                .WithUsing("FluentValidation")
                .WithUsing("Microsoft.Extensions.Logging")
                .WithUsing("MediatR")
                .WithUsing("System")
                .WithUsing("System.Threading")
                .WithUsing("System.Threading.Tasks")
                .WithUsing($"{_domainNamespace.PascalCase}.Models")
                .WithUsing($"{_domainNamespace.PascalCase}.Core")
                .WithUsing($"{_applicationNamespace.PascalCase}.Interfaces")
                .WithClass(validator)
                .WithClass(request)
                .WithClass(response)
                .WithClass(handler)
                .Build();

        }
    }
}
