using Endpoint.Core.Builders.Common;
using Endpoint.Core.Builders.Statements;
using Endpoint.Core.Models;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Builders.Core
{
    public class AggregateBuilder
    {
        public static void Build(string aggregate, List<ClassProperty> classProperties, Settings settings, IFileSystem fileSystem)
        {
            var resource = (Token)aggregate;

            var aggregateDirectory = $"{settings.ApplicationDirectory}{Path.DirectorySeparatorChar}AggregatesModel{Path.DirectorySeparatorChar}{resource.PascalCase}AggregateRoot";

            new ClassBuilder(resource.PascalCase, new Context(), fileSystem)
                .WithDirectory(aggregateDirectory)
                .WithUsing("System")
                .WithNamespace($"{settings.ApplicationNamespace}")
                .WithProperty(new PropertyBuilder().WithName(IdPropertyNameBuilder.Build(settings,resource.PascalCase)).WithType(IdDotNetTypeBuilder.Build(settings)).WithAccessors(new AccessorsBuilder().Build()).Build())
                .Build();

            new ClassBuilder($"{resource.PascalCase}Dto", new Context(), fileSystem)
                .WithDirectory(aggregateDirectory)
                .WithUsing("System")
                .WithNamespace($"{settings.ApplicationNamespace}")
                .WithProperty(new PropertyBuilder().WithName($"{resource.PascalCase}Id").WithType("Guid?").WithAccessors(new AccessorsBuilder().Build()).Build())
                .Build();

            new ClassBuilder($"{resource.PascalCase}Extensions", new Context(), fileSystem)
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
                .WithMethod(new MethodBuilder()
                .IsStatic()
                .WithName("ToDtos")
                .WithReturnType($"List<{resource.PascalCase}Dto>")
                .WithPropertyName($"{resource.PascalCase}Id")
                .WithParameter(new ParameterBuilder($"IEnumerable<{resource.PascalCase}>", resource.CamelCasePlural, true).Build())
                .WithBody(new()
                {
                    $"return {resource.CamelCasePlural}.Select(x => x.ToDto()).ToList();"
                })
                .Build())
                .Build();

            new ClassBuilder($"{resource.PascalCase}Validator", new Context(), fileSystem)
                .WithDirectory(aggregateDirectory)
                .WithBase(new TypeBuilder().WithGenericType("AbstractValidator", $"{resource.PascalCase}Dto").Build())
                .WithNamespace($"{settings.ApplicationNamespace}")
                .WithUsing("FluentValidation")
                .Build();
        }
    }
}
