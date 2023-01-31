using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.Entities.Legacy;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.RouteHandlers;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Services;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Endpoint.Core.Models.Artifacts.Files.Factories;

using IFileProvider = Services.IFileProvider;
public class FileModelFactory : IFileModelFactory
{
    private readonly IRouteHandlerModelFactory _routeHandlerModelFactory;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ILegacyAggregateModelFactory _aggregateRootModelFactory;
    private readonly IFileProvider _fileProvider;
    private readonly INamespaceProvider _namespaceProvider;

    public FileModelFactory(
        ILegacyAggregateModelFactory aggregateRootModelFactory,
        IRouteHandlerModelFactory routeHandlerModelFactory,
        INamingConventionConverter namingConventionConverter,
        IFileProvider fileProvider,
        INamespaceProvider namespaceProvider)
    {
        _aggregateRootModelFactory = aggregateRootModelFactory;
        _routeHandlerModelFactory = routeHandlerModelFactory;
        _namingConventionConverter = namingConventionConverter;
        _fileProvider = fileProvider;
        _namespaceProvider = namespaceProvider;
    }

    public TemplatedFileModel CreateTemplate(string template, string name, string directory, string extension = "cs", string filename = null, Dictionary<string, object> tokens = null)
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
        return new TemplatedFileModel("LaunchSettings", "LaunchSettings", projectDirectory, "json", new TokensBuilder()
            .With(nameof(projectName), (SyntaxToken)projectName)
            .With(nameof(port), (SyntaxToken)$"{port}")
            .With("SslPort", (SyntaxToken)$"{port + 1}")
            .Build());
    }

    public TemplatedFileModel AppSettings(string projectDirectory, string projectName, string dbContextName)
    {
        return new TemplatedFileModel("AppSettings", "appSettings", projectDirectory, "json", new TokensBuilder()
            .With(nameof(dbContextName), (SyntaxToken)dbContextName)
            .With("Namespace", (SyntaxToken)projectName)
            .Build());
    }

    public FileModel CreateCSharp<T>(T classModel, string directory)
        where T : TypeDeclarationModel
        => new ObjectFileModel<T>(classModel, classModel.UsingDirectives, classModel.Name, directory, "cs");

    public FileModel CreateResponseBase(string directory)
    {
        var classModel = new ClassModel("ResponseBase");

        classModel.Constructors.Add(new ConstructorModel(classModel, classModel.Name)
        {
            Body = "Errors = new List<string>();"
        });

        classModel.Properties.Add(new PropertyModel(classModel, AccessModifier.Public, new TypeModel("List")
        {
            GenericTypeParameters = new List<TypeModel>
            {
                new TypeModel("string")
            }

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
                new TypeModel("T")
            }
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
                        new TypeModel("T")
                    }
                }
            },
            new ParamModel()
            {
                Name = "pageIndex",
                Type = new TypeModel("int")
            },
            new ParamModel()
            {
                Name = "pageSize",
                Type = new TypeModel("int")
            }
        };

        methodModel.Body = "return queryable.Skip(pageSize * pageIndex).Take(pageSize);";

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
            "FluentValidation"
        }.Select(x => $"global using {x};");

        return new ContentFileModel(new StringBuilder()
            .AppendJoin(Environment.NewLine, usings)
            .ToString(), "Usings", directory, "cs");
    }
    public FileModel CreateDbContextInterface(string diretory)
    {

        var entities = new List<EntityModel>();

        var projectPath = _fileProvider.Get("*.csproj", diretory);

        var projectDirectory = Path.GetDirectoryName(projectPath);


        var serviceName = Path.GetFileNameWithoutExtension(projectDirectory).Split('.')[0];

        var usingDirectives = new List<UsingDirectiveModel>();

        foreach (var folder in Directory.GetDirectories($"{projectDirectory}{Path.DirectorySeparatorChar}AggregatesModel"))
        {
            var folderName = Path.GetFileNameWithoutExtension(folder);

            if (folderName.EndsWith("Aggregate"))
            {
                entities.Add(new EntityModel(folderName.Remove("Aggregate")));

                usingDirectives.Add(new UsingDirectiveModel() { Name = _namespaceProvider.Get(folder) });
            }

        }
        var interfaceModel = new DbContextModel(_namingConventionConverter, $"{serviceName}DbContext", entities, serviceName)
        {
            UsingDirectives = usingDirectives
        }.ToInterface();


        return new ObjectFileModel<InterfaceModel>(interfaceModel, interfaceModel.UsingDirectives, interfaceModel.Name, projectDirectory, "cs");

    }
}
