// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Builders;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Services;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Core
{
    public class DbContextInterfaceBuilder
    {
        public static void Default(SettingsModel settings, IFileSystem fileSystem)
        {
            var classBuilder = new ClassBuilder(settings.DbContextName, new Endpoint.Core.Services.Context(), fileSystem, "interface")
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

            foreach (var resource in settings.Resources.Select(x => (SyntaxToken)x.Name))
            {
                classBuilder.WithProperty(new PropertyBuilder().WithName(resource.PascalCasePlural()).WithAccessModifier(AccessModifier.Inherited).WithType(new TypeBuilder().WithGenericType("DbSet", resource.PascalCase()).Build()).WithAccessors(new AccessorsBuilder().WithGetterOnly().Build()).Build());
            }

            classBuilder.Build();
        }
    }
}

