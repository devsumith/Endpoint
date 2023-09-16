// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Constructors;
using Endpoint.Core.Syntax.Entities;
using Endpoint.Core.Syntax.Interfaces;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.RouteHandlers;
using Endpoint.Core.Syntax.Types;

namespace Endpoint.Core.Artifacts.Files.Factories;

using IFileProvider = IFileProvider;
public class FileFactory : IFileFactory
{
    private readonly IRouteHandlerFactory routeHandlerFactory;
    private readonly INamingConventionConverter namingConventionConverter;
    private readonly dynamic aggregateRootFactory;
    private readonly IFileProvider fileProvider;
    private readonly INamespaceProvider namespaceProvider;

    public FileFactory(
        dynamic aggregateRootFactory,
        IRouteHandlerFactory routeHandlerFactory,
        INamingConventionConverter namingConventionConverter,
        IFileProvider fileProvider,
        INamespaceProvider namespaceProvider)
    {
        this.aggregateRootFactory = aggregateRootFactory;
        this.routeHandlerFactory = routeHandlerFactory;
        this.namingConventionConverter = namingConventionConverter;
        this.fileProvider = fileProvider;
        this.namespaceProvider = namespaceProvider;
    }

    public TemplatedFileModel CreateTemplate(string template, string name, string directory, string extension = ".cs", string filename = null, Dictionary<string, object> tokens = null)
    {
        return new TemplatedFileModel(template, name, directory, extension, tokens);
    }

    public EntityFileModel Create(EntityModel model, string directory)
    {
        return new EntityFileModel(model, directory);
    }

    public CSharpTemplatedFileModel CreateCSharp(string template, string @namespace, string name, string directory, Dictionary<string, object> tokens = null)
    {
        return new CSharpTemplatedFileModel(template, @namespace, name, directory, tokens ?? new TokensBuilder()
            .With("Name", (SyntaxToken)name)
            .With("Namespace", (SyntaxToken)@namespace)
            .Build());
    }

    public TemplatedFileModel LaunchSettingsJson(string projectDirectory, string projectName, int port)
    {
        return new TemplatedFileModel("LaunchSettings", "LaunchSettings", projectDirectory, ".json", new TokensBuilder()
            .With(nameof(projectName), (SyntaxToken)projectName)
            .With(nameof(port), (SyntaxToken)$"{port}")
            .With("SslPort", (SyntaxToken)$"{port + 1}")
            .Build());
    }

    public TemplatedFileModel AppSettings(string projectDirectory, string projectName, string dbContextName)
    {
        return new TemplatedFileModel("AppSettings", "appSettings", projectDirectory, ".json", new TokensBuilder()
            .With(nameof(dbContextName), (SyntaxToken)dbContextName)
            .With("Namespace", (SyntaxToken)projectName)
            .Build());
    }

    public FileModel CreateCSharp<T>(T classModel, string directory)
        where T : TypeDeclarationModel
        => new CodeFileModel<T>(classModel, classModel.Usings, classModel.Name, directory, ".cs");

    public FileModel CreateResponseBase(string directory)
    {
        var classModel = new ClassModel("ResponseBase");

        classModel.Constructors.Add(new ConstructorModel(classModel, classModel.Name)
        {
            Body = "Errors = new List<string>();",
        });

        classModel.Properties.Add(new PropertyModel(classModel, AccessModifier.Public, new TypeModel("List")
        {
            GenericTypeParameters = new List<TypeModel>
            {
                new TypeModel("string"),
            },
        }, "Errors", PropertyAccessorModel.GetSet));

        return CreateCSharp(classModel, directory);
    }

    public FileModel CreateLinqExtensions(string directory)
    {
        var classModel = new ClassModel("LinqExtensions");

        classModel.Static = true;

        var methodModel = new MethodModel();

        methodModel.Name = "Page<T>";

        methodModel.ParentType = classModel;

        methodModel.ReturnType = new TypeModel("IQueryable")
        {
            GenericTypeParameters = new List<TypeModel>
            {
                new TypeModel("T"),
            },
        };

        methodModel.Params = new List<ParamModel>
        {
            new ParamModel()
            {
                ExtensionMethodParam = true,
                Name = "queryable",
                Type = new TypeModel("IQueryable")
                {
                    GenericTypeParameters = new List<TypeModel>
                    {
                        new TypeModel("T"),
                    },
                },
            },
            new ParamModel()
            {
                Name = "pageIndex",
                Type = new TypeModel("int"),
            },
            new ParamModel()
            {
                Name = "pageSize",
                Type = new TypeModel("int"),
            },
        };

        methodModel.Body = new Syntax.Expressions.ExpressionModel("return queryable.Skip(pageSize * pageIndex).Take(pageSize);");

        classModel.Methods.Add(methodModel);

        return CreateCSharp(classModel, directory);
    }

    public FileModel CreateCoreUsings(string directory)
    {
        var usings = new string[]
        {
            "MediatR",
            "Microsoft.Extensions.Logging",
            "Microsoft.EntityFrameworkCore",
            "FluentValidation",
        }.Select(x => $"global using {x};");

        return new ContentFileModel(
            new StringBuilder()
            .AppendJoin(Environment.NewLine, usings)
            .ToString(), "Usings", directory, ".cs");
    }

    public FileModel CreateDbContextInterface(string diretory)
    {
        var entities = new List<EntityModel>();

        var projectPath = fileProvider.Get("*.csproj", diretory);

        var projectDirectory = Path.GetDirectoryName(projectPath);

        var serviceName = Path.GetFileNameWithoutExtension(projectDirectory).Split('.')[0];

        var usingDirectives = new List<UsingModel>();

        foreach (var folder in Directory.GetDirectories($"{projectDirectory}{Path.DirectorySeparatorChar}AggregatesModel"))
        {
            var folderName = Path.GetFileNameWithoutExtension(folder);

            if (folderName.EndsWith("Aggregate"))
            {
                entities.Add(new EntityModel(folderName.Remove("Aggregate")));

                usingDirectives.Add(new (namespaceProvider.Get(folder)));
            }
        }

        var interfaceModel = new DbContextModel(namingConventionConverter, $"{serviceName}DbContext", entities, serviceName)
        {
            Usings = usingDirectives,
        }.ToInterface();

        return new CodeFileModel<InterfaceModel>(interfaceModel, interfaceModel.Usings, interfaceModel.Name, projectDirectory, ".cs");
    }
}
