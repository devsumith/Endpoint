// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using System.Collections.Generic;

namespace Endpoint.Core.Builders
{
    public class CommandBuilder
    {
        public static void Build(SettingsModel settings, SyntaxToken name, Context context, IFileSystem fileSystem, string directory, string @namespace)
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
                .WithDependency($"I{((SyntaxToken)settings.DbContextName).PascalCase}", "context")
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
                .WithUsing($"{((SyntaxToken)settings.ApplicationNamespace).PascalCase}")
                .WithUsing($"{((SyntaxToken)settings.DomainNamespace).PascalCase}.Core")
                .WithUsing($"{((SyntaxToken)settings.ApplicationNamespace).PascalCase}.Interfaces")
                .WithClass(validator)
                .WithClass(request)
                .WithClass(response)
                .WithClass(handler)
                .Build();

        }
    }
}

