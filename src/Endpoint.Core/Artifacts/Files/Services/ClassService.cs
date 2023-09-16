// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Attributes;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.Types;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Files.Services;

public class ClassService : IClassService
{
    private readonly ILogger<ClassService> logger;
    private readonly IFileSystem fileSystem;
    private readonly IFileProvider fileProvider;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly INamespaceProvider nameSpaceProvider;

    public ClassService(
        ILogger<ClassService> logger,
        IArtifactGenerator artifactGenerator,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        INamespaceProvider namespaceProvider)
    {
        this.artifactGenerator = artifactGenerator;
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        nameSpaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
    }

    public async Task CreateAsync(string name, string properties, string directory)
    {
        logger.LogInformation("Create Class {name}", name);

        var @class = new ClassModel(name);

        @class.Usings.Add(new ("System"));

        if (!string.IsNullOrEmpty(properties))
        {
            foreach (var property in properties.Split(','))
            {
                var parts = property.Split(':');
                var propertyName = parts[0];
                var propertyType = parts[1];

                @class.Properties.Add(new PropertyModel(@class, AccessModifier.Public, new TypeModel() { Name = propertyType }, propertyName, new List<PropertyAccessorModel>()));
            }
        }

        var classFile = new CodeFileModel<ClassModel>(
            @class,
            @class.Usings,
            @class.Name,
            directory,
            ".cs");

        await artifactGenerator.GenerateAsync(classFile);
    }

    public async Task UnitTestCreateAsync(string name, string methods, string directory)
    {
        logger.LogInformation("Create Unit Test for {name}", name);

        var projectDirectory = Path.GetDirectoryName(fileProvider.Get("*.csproj", directory));

        var slnDirectory = Path.GetDirectoryName(fileProvider.Get("*.sln", directory));

        var classPath = Directory.GetFiles(slnDirectory, $"{name}.cs", SearchOption.AllDirectories).FirstOrDefault();

        if (classPath == null)
        {
            foreach (var path in Directory.GetFiles(Path.GetDirectoryName(projectDirectory), "*.cs", SearchOption.AllDirectories))
            {
                if (fileSystem.File.ReadAllText(path).Contains($"class {name}"))
                {
                    classPath = path;
                    break;
                }
            }
        }

        if (classPath == null)
        {
            foreach (var path in Directory.GetFiles(Path.GetDirectoryName(Path.GetDirectoryName(projectDirectory)), "*.cs", SearchOption.AllDirectories))
            {
                if (fileSystem.File.ReadAllText(path).Contains($"class {name}"))
                {
                    classPath = path;
                    break;
                }
            }
        }

        fileSystem.Directory.CreateDirectory($"{projectDirectory}{Path.DirectorySeparatorChar}{name}");

        foreach (var methodModel in Parse(name, classPath))
        {
            await CreateMethodTestFile(methodModel.Name);
        }

        if (!string.IsNullOrEmpty(methods))
        {
            foreach (var methodName in methods.Split(','))
            {
                await CreateMethodTestFile(methodName);
            }
        }

        async Task CreateMethodTestFile(string methodName)
        {
            var classModel = new ClassModel($"{methodName}Should");

            var fact = new MethodModel()
            {
                Name = "DoSomething_GivenSomething",
                ReturnType = new TypeModel("void"),
                Attributes = new List<AttributeModel>()
                {
                    new AttributeModel()
                    {
                        Type = AttributeType.Fact,
                        Name = "Fact",
                    },
                },
                Body = new Syntax.Expressions.ExpressionModel(string.Join(Environment.NewLine, new string[] { "ARRANGE", "ACT", "ASSERT" }.Select(x => $"// {x}{Environment.NewLine}"))),
            };

            classModel.Methods.Add(fact);

            classModel.Usings.Add(new ("Xunit"));

            classModel.Usings.Add(new (nameSpaceProvider.Get(Path.GetDirectoryName(classPath))));

            classModel.UsingAs.Add(new UsingAsModel($"{nameSpaceProvider.Get(Path.GetDirectoryName(classPath))}.{name}", name));

            await artifactGenerator.GenerateAsync(new CodeFileModel<ClassModel>(classModel, classModel.Usings, classModel.Name, $"{projectDirectory}{Path.DirectorySeparatorChar}{name}", ".cs"));
        }
    }

    public List<MethodModel> Parse(string className, string path)
    {
        var methods = new List<MethodModel>();

        var insideClass = false;

        foreach (var line in fileSystem.File.ReadAllLines(path))
        {
            if (insideClass)
            {
                if (line.Trim().StartsWith("public") && !line.Trim().StartsWith($"public {className}"))
                {
                    var indexOfEndOfName = line.IndexOf('(') - 1;

                    for (var i = indexOfEndOfName; i > 0; i--)
                    {
                        if (line.ToCharArray().ElementAt(i) == ' ')
                        {
                            var methodName = line.Substring(i + 1, indexOfEndOfName - i);

                            methods.Add(new MethodModel()
                            {
                                Name = methodName,
                            });

                            i = -1;
                        }
                    }
                }
            }

            if (line.Contains("class") && insideClass)
            {
                insideClass = false;
            }

            if (line.Contains($"class {className}"))
            {
                insideClass = true;
            }
        }

        return methods;
    }
}
