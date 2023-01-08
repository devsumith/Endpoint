using Endpoint.Core.Builders;
using Endpoint.Core.Builders.Common;
using Endpoint.Core.Builders.Statements;
using Endpoint.Core.Core;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Strategies.Application;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Services
{
    public class ApplicationProjectFilesGenerationStrategy : BaseProjectFilesGenerationStrategy, IApplicationProjectFilesGenerationStrategy
    {
        public ApplicationProjectFilesGenerationStrategy(ICommandService commandService, ITemplateProcessor templateProcessor, ITemplateLocator templateLocator, IFileSystem fileSystem)
            : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public void Build(SettingsModel settings)
        {
            _removeDefaultFiles(settings.ApplicationDirectory);

            _createFolder($"Interfaces", settings.ApplicationDirectory);

            _createFolder($"Behaviors", settings.ApplicationDirectory);

            _createFolder($"Extensions", settings.ApplicationDirectory);

            DbContextInterfaceBuilder.Default(settings, _fileSystem);

            foreach (var resource in settings.Resources.Select(x => x))
            {
                _buildApplicationFilesForResource(settings, resource);
            }

            _commandService.Start($"dotnet add package FluentValidation --version 10.3.6", $@"{settings.ApplicationDirectory}");
            _commandService.Start($"dotnet add package MediatR  --version 10.0.1", $@"{settings.ApplicationDirectory}");
            _commandService.Start($"dotnet add package StronglyTypedId  --version 1.0.0-beta05", $@"{settings.ApplicationDirectory}");
            _commandService.Start($"dotnet add package Newtonsoft.Json  --version 13.0.1", $@"{settings.ApplicationDirectory}");

        }

        protected void _buildApplicationFilesForResource(SettingsModel settings, AggregateRootModel resource)
        {
            Token resourceName = ((Token)resource.Name);
            var aggregateDirectory = $"{settings.ApplicationDirectory}{Path.DirectorySeparatorChar}AggregatesModel{Path.DirectorySeparatorChar}{resourceName.PascalCase}Aggregate";
            var commandsDirectory = $"{aggregateDirectory}{Path.DirectorySeparatorChar}Commands";
            var queriesDirectory = $"{aggregateDirectory}{Path.DirectorySeparatorChar}Queries";
            var context = new Context();


            _createFolder(commandsDirectory, settings.ApplicationDirectory);

            _createFolder(queriesDirectory, settings.ApplicationDirectory);


            if(settings.IdDotNetType == IdPropertyType.Guid)
            {
                new IdFileGenerationStrategy(_templateProcessor, _templateLocator, _fileSystem)
                    .Create(resourceName.Value, settings.ApplicationNamespace, aggregateDirectory);
            }

            var aggregateBuilder = new ClassBuilder(resource.Name, new Context(), _fileSystem)
                .WithDirectory(aggregateDirectory)
                .WithUsing("System")
                .WithNamespace($"{settings.ApplicationNamespace}");

            foreach (var property in resource.Properties)
            {
                if(property.Id)
                {
                    var syntax = new PropertyBuilder().WithName(IdPropertyNameBuilder.Build(settings, resourceName)).WithType(IdDotNetTypeBuilder.Build(settings, resource.Name, true)).WithAccessors(new AccessorsBuilder().Build()).Build(settings, resourceName);

                    
                    aggregateBuilder.WithProperty(syntax);

                    
                }
                else
                {
                    aggregateBuilder.WithProperty(new PropertyBuilder().WithName(property.Name).WithType(property.Type).WithAccessors(new AccessorsBuilder().Build()).Build());
                }
            }

            aggregateBuilder.Build();

            var dtoBuilder = new ClassBuilder($"{((Token)resource.Name).PascalCase}Dto", new Context(), _fileSystem)
                .WithDirectory(aggregateDirectory)
                .WithUsing("System")
                .WithNamespace($"{settings.ApplicationNamespace}");

            foreach (var property in resource.Properties)
            {
                if (property.Id)
                {
                    dtoBuilder.WithProperty(new PropertyBuilder().WithName(IdPropertyNameBuilder.Build(settings, resourceName)).WithType($"{IdDotNetTypeBuilder.Build(settings, resource.Name)}?").WithAccessors(new AccessorsBuilder().Build()).Build());
                }
                else
                {
                    dtoBuilder.WithProperty(new PropertyBuilder().WithName(property.Name).WithType(property.Type).WithAccessors(new AccessorsBuilder().Build()).Build());
                }
            }

            dtoBuilder.Build();


            var extensionsBody = new List<string>()
            {
                "return new ()",
                "{",
            };

            foreach (var property in resource.Properties)
            {
                if(property.Id && settings.IdDotNetType == IdPropertyType.Guid)
                {
                    extensionsBody.Add($"{property.Name} = {((Token)resource.Name).CamelCase}.{((Token)property.Name).PascalCase}.Value,".Indent(1));
                } else
                {
                    extensionsBody.Add($"{property.Name} = {((Token)resource.Name).CamelCase}.{((Token)property.Name).PascalCase},".Indent(1));
                }
                
            }

            extensionsBody.Add("};");


            new ClassBuilder($"{((Token)resource.Name).PascalCase}Extensions", new Context(), _fileSystem)
                .WithDirectory(aggregateDirectory)
                .IsStatic()
                .WithUsing("System.Collections.Generic")
                .WithUsing("Microsoft.EntityFrameworkCore")
                .WithUsing("System.Linq")
                .WithUsing("System.Threading.Tasks")
                .WithUsing("System.Threading")
                .WithNamespace(settings.ApplicationNamespace)
                .WithMethod(new MethodBuilder()
                .IsStatic()
                .WithName("ToDto")
                .WithReturnType($"{((Token)resource.Name).PascalCase}Dto")
                .WithPropertyName($"{((Token)resource.Name).PascalCase}Id")
                .WithParameter(new ParameterBuilder(((Token)resource.Name).PascalCase, ((Token)resource.Name).CamelCase, true).Build())
                .WithBody(extensionsBody)
                .Build())
                .WithMethod(new MethodBuilder()
                .IsStatic()
                .WithAsync(true)
                .WithName("ToDtosAsync")
                .WithReturnType($"Task<List<{((Token)resource.Name).PascalCase}Dto>>")
                .WithPropertyName($"{((Token)resource.Name).PascalCase}Id")
                .WithParameter(new ParameterBuilder($"IQueryable<{((Token)resource.Name).PascalCase}>", ((Token)resource.Name).CamelCasePlural, true).Build())
                .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build())
                .WithBody(new()
                {
                    $"return await {resourceName.CamelCasePlural}.Select(x => x.ToDto()).ToListAsync(cancellationToken);"
                })
                .Build())
                .WithMethod(new MethodBuilder()
                .IsStatic()
                .WithName("ToDtos")
                .WithReturnType($"List<{resourceName.PascalCase}Dto>")
                .WithPropertyName($"{resourceName.PascalCase}Id")
                .WithParameter(new ParameterBuilder($"IEnumerable<{resourceName.PascalCase}>", resourceName.CamelCasePlural, true).Build())
                .WithBody(new()
                {
                    $"return {resourceName.CamelCasePlural}.Select(x => x.ToDto()).ToList();"
                })
                .Build())
                .Build();

            new ClassBuilder($"{resourceName.PascalCase}Validator", new Context(), _fileSystem)
                .WithDirectory(aggregateDirectory)
                .WithBase(new TypeBuilder().WithGenericType("AbstractValidator", $"{resourceName.PascalCase}Dto").Build())
                .WithNamespace($"{settings.ApplicationNamespace}")
                .WithUsing("FluentValidation")
                .Build();

            new CreateBuilder(new Context(), _fileSystem)
                .WithDirectory(commandsDirectory)
                .WithDbContext(settings.DbContextName)
                .WithNamespace($"{settings.ApplicationNamespace}")
                .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                .WithDomainNamespace($"{settings.DomainNamespace}")
                .WithEntity(resourceName.Value)
                .WithAggregateRoot(resource)
                .Build();

            new UpdateBuilder(settings, new Context(), _fileSystem)
                .WithDirectory(commandsDirectory)
                .WithDbContext(settings.DbContextName)
                .WithNamespace($"{settings.ApplicationNamespace}")
                .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                .WithDomainNamespace($"{settings.DomainNamespace}")
                .WithEntity(resourceName.Value)
                .WithAggregateRoot(resource)
                .Build();

            new RemoveBuilder(settings, new Context(), _fileSystem)
                .WithDirectory(commandsDirectory)
                .WithDbContext(settings.DbContextName)
                .WithNamespace($"{settings.ApplicationNamespace}")
                .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                .WithDomainNamespace($"{settings.DomainNamespace}")
                .WithEntity(resourceName.Value)
                .Build();

            new GetByIdBuilder(settings, new Context(), _fileSystem)
                .WithDirectory(queriesDirectory)
                .WithDbContext(settings.DbContextName)
                .WithNamespace($"{settings.ApplicationNamespace}")
                .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                .WithDomainNamespace($"{settings.DomainNamespace}")
                .WithEntity(resourceName.Value)
                .Build();

            new GetBuilder(new Context(), _fileSystem)
                .WithDirectory(queriesDirectory)
                .WithDbContext(settings.DbContextName)
                .WithNamespace($"{settings.ApplicationNamespace}")
                .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                .WithDomainNamespace($"{settings.DomainNamespace}")
                .WithEntity(resourceName.Value)
                .Build();

            new GetPageBuilder(new Context(), _fileSystem)
                .WithDirectory(queriesDirectory)
                .WithDbContext(settings.DbContextName)
                .WithNamespace($"{settings.ApplicationNamespace}")
                .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                .WithDomainNamespace($"{settings.DomainNamespace}")
                .WithEntity(resourceName.Value)
                .Build();

            _buildValidationBehavior(settings);
            _buildServiceCollectionExtensions(settings);
        }

        public void BuildAdditionalResource(AggregateRootModel aggregateModel, SettingsModel settings)
        {
            DbContextInterfaceBuilder.Default(settings, _fileSystem);

            _buildApplicationFilesForResource(settings, aggregateModel);
        }

        private void _buildValidationBehavior(SettingsModel settings)
        {
            var template = _templateLocator.Get(nameof(ValidationBehaviorBuilder));

            var tokens = new TokensBuilder()
                .With(nameof(settings.ApplicationNamespace), (Token)settings.ApplicationNamespace)
                .With(nameof(settings.DomainNamespace), (Token)settings.DomainNamespace)
                .Build();

            var contents = string.Join(Environment.NewLine,_templateProcessor.Process(template, tokens));

            _fileSystem.WriteAllText($@"{settings.ApplicationDirectory}{Path.DirectorySeparatorChar}Behaviors{Path.DirectorySeparatorChar}ValidationBehavior.cs", contents);
        }

        private void _buildServiceCollectionExtensions(SettingsModel settings)
        {
            var template = _templateLocator.Get("ConfigureValidationServices");

            var tokens = new TokensBuilder()
                .With(nameof(settings.ApplicationNamespace), (Token)settings.ApplicationNamespace)
                .With(nameof(settings.DomainNamespace), (Token)settings.DomainNamespace)
                .Build();

            var contents = string.Join(Environment.NewLine,_templateProcessor.Process(template, tokens));

            _fileSystem.WriteAllText($@"{settings.ApplicationDirectory}{Path.DirectorySeparatorChar}Extensions{Path.DirectorySeparatorChar}ServiceCollectionExtensions.cs", contents);
        }


    }
}
