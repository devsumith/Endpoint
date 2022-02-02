using Endpoint.Application.Builders;
using Endpoint.Application.Enums;
using Endpoint.Application.Extensions;
using Endpoint.Application.Models;
using Endpoint.Application.ValueObjects;
using System.IO;
using System.Linq;

namespace Endpoint.Application.Services.FileServices
{
    public class ApplicationFileService : BaseFileService, IApplicationFileService
    {
        public ApplicationFileService(ICommandService commandService, ITemplateProcessor templateProcessor, ITemplateLocator templateLocator, IFileSystem fileSystem) 
            : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        public void Build(Settings settings)
        {
            _removeDefaultFiles(settings.ApplicationDirectory);

            _createFolder($"Interfaces", settings.ApplicationDirectory);

            var dbContextInterfaceBuilder = new ClassBuilder(settings.DbContextName, new Context(), _fileSystem, "interface")
                .WithDirectory($"{settings.ApplicationDirectory}{Path.DirectorySeparatorChar}Interfaces")                
                .WithUsing("Microsoft.EntityFrameworkCore")
                .WithUsing("System.Threading.Tasks")
                .WithUsing("System.Threading")
                .WithNamespace($"{settings.ApplicationNamespace}.Interfaces")
                .WithMethodSignature(new MethodSignatureBuilder()
                .WithAsync(false)
                .WithAccessModifier(AccessModifier.Inherited)
                .WithName("SaveChangesAsync")
                .WithReturnType(new TypeBuilder().WithGenericType("Task", "int").Build())
                .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build()).Build());

            foreach (var resource in settings.Resources.Select(x => (Token)x))
            {                
                var aggregateDirectory = $"{settings.ApplicationDirectory}{Path.DirectorySeparatorChar}{resource.PascalCase}Aggregate";
                var commandsDirectory = $"{aggregateDirectory}{Path.DirectorySeparatorChar}Commands";
                var queriesDirectory = $"{aggregateDirectory}{Path.DirectorySeparatorChar}Queries";
                var context = new Context();

                _createFolder(commandsDirectory, settings.ApplicationDirectory);
                _createFolder(queriesDirectory, settings.ApplicationDirectory);

                dbContextInterfaceBuilder.WithProperty(new PropertyBuilder().WithName(resource.PascalCasePlural).WithAccessModifier(AccessModifier.Inherited).WithType(new TypeBuilder().WithGenericType("DbSet", resource.PascalCase).Build()).WithAccessors(new AccessorsBuilder().WithGetterOnly().Build()).Build());

                dbContextInterfaceBuilder.Build();

                new ClassBuilder(resource.PascalCase, new Context(), _fileSystem)
                    .WithDirectory(aggregateDirectory)
                    .WithUsing("System")
                    .WithNamespace($"{settings.ApplicationNamespace}")
                    .WithProperty(new PropertyBuilder().WithName($"{resource.PascalCase}Id").WithType("Guid").WithAccessors(new AccessorsBuilder().Build()).Build())
                    .Build();

                new ClassBuilder($"{resource.PascalCase}Dto", new Context(), _fileSystem)
                    .WithDirectory(aggregateDirectory)
                    .WithUsing("System")
                    .WithNamespace($"{settings.ApplicationNamespace}")
                    .WithProperty(new PropertyBuilder().WithName($"{resource.PascalCase}Id").WithType("Guid").WithAccessors(new AccessorsBuilder().Build()).Build())
                    .Build();

                new ClassBuilder($"{resource.PascalCase}Extensions", new Context(), _fileSystem)
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
                    .WithReturnType($"{resource.PascalCase}Dto")
                    .WithPropertyName($"{resource.PascalCase}Id")
                    .WithParameter(new ParameterBuilder(resource.PascalCase, resource.CamelCase, true).Build())
                    .WithBody(new()
                    {
                        "return new ()",
                        "{",
                        $"{resource.PascalCase}Id = {resource.CamelCase}.{resource.PascalCase}Id".Indent(1),
                        "};"
                    })
                    .Build())
                    .WithMethod(new MethodBuilder()
                    .IsStatic()
                    .WithAsync(true)
                    .WithName("ToDtosAsync")
                    .WithReturnType($"Task<List<{resource.PascalCase}Dto>>")
                    .WithPropertyName($"{resource.PascalCase}Id")
                    .WithParameter(new ParameterBuilder($"IQueryable<{resource.PascalCase}>", resource.CamelCasePlural, true).Build())
                    .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build())
                    .WithBody(new()
                    {
                        $"return await {resource.CamelCasePlural}.Select(x => x.ToDto()).ToListAsync(cancellationToken);"
                    })
                    .Build())
                    .Build();

                new ClassBuilder($"{resource.PascalCase}Validator", new Context(), _fileSystem)
                    .WithDirectory(aggregateDirectory)
                    .WithBase(new TypeBuilder().WithGenericType("AbstractValidator", $"{resource.PascalCase}Dto").Build())
                    .WithNamespace($"{settings.ApplicationNamespace}")
                    .WithUsing("FluentValidation")
                    .Build();

                new CreateBuilder(new Context(), _fileSystem)
                    .WithDirectory(commandsDirectory)
                    .WithDbContext(settings.DbContextName)
                    .WithNamespace($"{settings.ApplicationNamespace}")
                    .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                    .WithDomainNamespace($"{settings.DomainNamespace}")
                    .WithEntity(resource.Value)
                    .Build();

                new UpdateBuilder(new Context(), _fileSystem)
                    .WithDirectory(commandsDirectory)
                    .WithDbContext(settings.DbContextName)
                    .WithNamespace($"{settings.ApplicationNamespace}")
                    .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                    .WithDomainNamespace($"{settings.DomainNamespace}")
                    .WithEntity(resource.Value)
                    .Build();

                new RemoveBuilder(new Context(), _fileSystem)
                    .WithDirectory(commandsDirectory)
                    .WithDbContext(settings.DbContextName)
                    .WithNamespace($"{settings.ApplicationNamespace}")
                    .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                    .WithDomainNamespace($"{settings.DomainNamespace}")
                    .WithEntity(resource.Value)
                    .Build();

                new GetByIdBuilder(new Context(), _fileSystem)
                    .WithDirectory(queriesDirectory)
                    .WithDbContext(settings.DbContextName)
                    .WithNamespace($"{settings.ApplicationNamespace}")
                    .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                    .WithDomainNamespace($"{settings.DomainNamespace}")
                    .WithEntity(resource.Value)
                    .Build();

                new GetBuilder(new Context(), _fileSystem)
                    .WithDirectory(queriesDirectory)
                    .WithDbContext(settings.DbContextName)
                    .WithNamespace($"{settings.ApplicationNamespace}")
                    .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                    .WithDomainNamespace($"{settings.DomainNamespace}")
                    .WithEntity(resource.Value)
                    .Build();

                new GetPageBuilder(new Context(), _fileSystem)
                    .WithDirectory(queriesDirectory)
                    .WithDbContext(settings.DbContextName)
                    .WithNamespace($"{settings.ApplicationNamespace}")
                    .WithApplicationNamespace($"{settings.ApplicationNamespace}")
                    .WithDomainNamespace($"{settings.DomainNamespace}")
                    .WithEntity(resource.Value)
                    .Build();
            }

            dbContextInterfaceBuilder.Build();

            _commandService.Start($"dotnet add package FluentValidation --version 10.3.3", $@"{settings.ApplicationDirectory}");
            _commandService.Start($"dotnet add package MediatR  --version 9.0.0", $@"{settings.ApplicationDirectory}");

        }
    }
}
