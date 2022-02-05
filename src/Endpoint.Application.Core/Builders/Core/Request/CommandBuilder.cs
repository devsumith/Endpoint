using Endpoint.SharedKernal.Models;
using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
using System.Collections.Generic;

namespace Endpoint.Application.Builders
{
    public class CommandBuilder
    {
        public static void Build(Settings settings, Token name, Context context, IFileSystem fileSystem, string directory, string @namespace)
        {

            var validator = new ClassBuilder($"{name.PascalCase}Validator", context, fileSystem)
                .WithBase(new TypeBuilder().WithGenericType("AbstractValidator", $"{name.PascalCase}Request").Build())
                .Class;

            var request = new ClassBuilder($"{name.PascalCase}Request", context, fileSystem)
                .WithInterface(new TypeBuilder().WithGenericType("IRequest", $"{name.PascalCase}Response").Build())
                .Class;

            var response = new ClassBuilder($"{name.PascalCase}Response", context, fileSystem)
                .WithBase("ResponseBase")
                .Class;

            var handler = new ClassBuilder($"{name.PascalCase}Handler", context, fileSystem)
                .WithBase(new TypeBuilder().WithGenericType("IRequestHandler", $"{name.PascalCase}Request", $"{name.PascalCase}Response").Build())
                .WithDependency($"I{((Token)settings.DbContextName).PascalCase}", "context")
                .WithDependency($"ILogger<{name.PascalCase}Handler>", "logger")
                .WithMethod(new MethodBuilder().WithName("Handle").WithAsync(true)
                .WithReturnType(new TypeBuilder().WithGenericType("Task", $"{name.PascalCase}Response").Build())
                .WithParameter(new ParameterBuilder($"{name.PascalCase}Request", "request").Build())
                .WithParameter(new ParameterBuilder("CancellationToken", "cancellationToken").Build())
                .WithBody(new List<string>() {
            "return new ()",
            "{",
            "};"
                }).Build())
                .Class;

            new NamespaceBuilder($"{name.PascalCase}", context, fileSystem)
                .WithDirectory(directory)
                .WithNamespace(@namespace)
                .WithUsing("FluentValidation")
                .WithUsing("Microsoft.Extensions.Logging")
                .WithUsing("MediatR")
                .WithUsing("System")
                .WithUsing("System.Threading")
                .WithUsing("System.Threading.Tasks")
                .WithUsing($"{((Token)settings.ApplicationNamespace).PascalCase}")
                .WithUsing($"{((Token)settings.DomainNamespace).PascalCase}.Core")
                .WithUsing($"{((Token)settings.ApplicationNamespace).PascalCase}.Interfaces")
                .WithClass(validator)
                .WithClass(request)
                .WithClass(response)
                .WithClass(handler)
                .Build();

        }
    }
}
