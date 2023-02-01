// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Internals;
using Endpoint.Core.Messages;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using MediatR;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Services;

public class DomainDrivenDesignFileService: IDomainDrivenDesignFileService {

    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly IFileProvider _fileProvider;
    private readonly Observable<INotification> _notificationListener;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ISyntaxGenerationStrategyFactory _syntaxGenerationStrategyFactory;
    private readonly IFileSystem _fileSystem;
    public DomainDrivenDesignFileService(
        IFileSystem fileSystem,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, 
        IFileProvider fileProvider,
        Observable<INotification> notificationListener,
        INamingConventionConverter namingConventionConverter,
        ISyntaxGenerationStrategyFactory syntaxGenerationStrategyFactory)
	{
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _notificationListener = notificationListener ?? throw new ArgumentNullException(nameof(notificationListener));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _syntaxGenerationStrategyFactory = syntaxGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(syntaxGenerationStrategyFactory));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
	}

    public void MessageCreate(string name, List<PropertyModel> properties, string directory)
    {
        var classModel = new ClassModel(name);

        classModel.Properties.AddRange(properties);

        classModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = "MediatR" });

        var constructorModel = new ConstructorModel(classModel, classModel.Name);

        foreach(var property in properties)
        {
            classModel.Fields.Add(new FieldModel()
            {
                Name = $"_{_namingConventionConverter.Convert(NamingConvention.CamelCase, property.Name)}",
                Type = property.Type
            });

            constructorModel.Params.Add(new ParamModel()
            {
                Name = $"{_namingConventionConverter.Convert(NamingConvention.CamelCase, property.Name)}",
                Type = property.Type
            });
        }

        classModel.Constructors.Add(constructorModel);

        classModel.Implements.Add(new TypeModel("INotification"));

        var classFileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, directory, "cs");

        _artifactGenerationStrategyFactory.CreateFor(classFileModel);

    }

    public void MessageHandlerCreate(string name, string directory)
    {
        var classModel = new ClassModel(name);

        var fields = new List<FieldModel>();

        var classParams = new List<ParamModel>();

        classModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = "MediatR" });

        classModel.Fields = fields;

        var constructorModel = new ConstructorModel(classModel, classModel.Name);

        foreach (var typeModel in new List<TypeModel> () {  })
        {
            classModel.Fields.Add(new FieldModel()
            {
                Name = $"_{_namingConventionConverter.Convert(NamingConvention.CamelCase, _syntaxGenerationStrategyFactory.CreateFor(typeModel))}",
                Type = typeModel
            });

            constructorModel.Params.Add(new ParamModel()
            {
                Name = $"{_namingConventionConverter.Convert(NamingConvention.CamelCase, _syntaxGenerationStrategyFactory.CreateFor(typeModel))}",
                Type = typeModel
            });
        }

        classModel.Constructors.Add(constructorModel);

        classModel.Implements.Add(new TypeModel("INotification"));

        var classFileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, directory, "cs");

        _artifactGenerationStrategyFactory.CreateFor(classFileModel);

    }

    public void ServiceCreate(string name, string directory)
	{
        if (_fileSystem.Exists($"{directory}{Path.DirectorySeparatorChar}{name}.cs"))
        {
            throw new Exception($"Service exists: {$"{directory}{Path.DirectorySeparatorChar}{name}.cs"}");
        }

        var usingDirectives = new List<UsingDirectiveModel>()
        {
            new UsingDirectiveModel() { Name = "Microsoft.Extensions.Logging" },
            new UsingDirectiveModel() { Name = "System" },
            new UsingDirectiveModel() { Name = "System.Threading.Tasks" }
        };

        var fields = new List<FieldModel>()
        {
            new FieldModel()
            {
                Name = "_logger",
                Type = new TypeModel()
                {
                    Name = "ILogger",
                    GenericTypeParameters = new List<TypeModel>()
                    {
                        new TypeModel() { Name = name}
                    }
                }
            }
        };

        var methods = new List<MethodModel>()
        {
            new MethodModel()
            {
                Name = "DoWorkAsync",
                ReturnType = new TypeModel("Task"),
                Async = true,
                Body = "_logger.LogInformation(\"DoWorkAsync\");"
            }
        };

        var @interface = createInterface(name, methods, usingDirectives, directory);

        _ = createClass(@interface,name,methods,usingDirectives, directory);

        InterfaceModel createInterface(string name, List<MethodModel> methods, List<UsingDirectiveModel> usings, string directory)
        {
            var @interface = new InterfaceModel($"I{name}");

            @interface.Methods = methods.Select(x => new MethodModel() { Name = x.Name, ReturnType = x.ReturnType, Async = x.Async, Interface = true }).ToList();

            @interface.UsingDirectives.AddRange(usings);

            var interfaceFile = new ObjectFileModel<InterfaceModel>(
                @interface,
                @interface.UsingDirectives,
                @interface.Name,
                directory,
                "cs"
                );

            _artifactGenerationStrategyFactory.CreateFor(interfaceFile);

            return @interface;
        }

        ClassModel createClass(InterfaceModel @interface, string name, List<MethodModel> methods, List<UsingDirectiveModel> usings, string directory)
        {
            var @class = new ClassModel(name);

            var constructors = new List<ConstructorModel>()
            {

                new ConstructorModel(@class,@class.Name)
                {
                    Params = new List<ParamModel>
                    {
                        new ParamModel()
                        {
                            Type = new TypeModel()
                            {
                                Name = "ILogger",
                                GenericTypeParameters = new List<TypeModel>()
                                {
                                    new TypeModel() { Name = name}
                                }
                            },
                            Name = "logger"
                        }
                    }
                }
            };
            @class.Constructors = constructors;

            @class.Methods = methods;

            @class.Fields = fields;

            @class.UsingDirectives.AddRange(usingDirectives);

            @class.Implements.Add(new TypeModel() { Name = @interface.Name });

            var classFile = new ObjectFileModel<ClassModel>(
                @class,
                @class.UsingDirectives,
                @class.Name,
                directory,
                "cs"
                );

            _artifactGenerationStrategyFactory.CreateFor(classFile);

            _notificationListener.Broadcast(new ServiceFileCreated(@interface.Name, @class.Name,directory));
            
            return @class;
        }
    }
}

