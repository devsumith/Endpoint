// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Internals;
using Endpoint.Core.Messages;
using Endpoint.Core.Artifacts.Files;
using MediatR;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Constructors;
using Endpoint.Core.Syntax.Fields;
using Endpoint.Core.Syntax.Interfaces;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.Types;
using Endpoint.Core.Syntax;

namespace Endpoint.Core.Services;

public class DomainDrivenDesignFileService : IDomainDrivenDesignFileService
{

    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IFileProvider _fileProvider;
    private readonly Observable<INotification> _notificationListener;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ISyntaxGenerator _syntaxGenerator;
    private readonly IFileSystem _fileSystem;
    private readonly INamespaceProvider _namespaceProvider;
    public DomainDrivenDesignFileService(
        INamespaceProvider namespaceProvider,
        IFileSystem fileSystem,
        IArtifactGenerator artifactGenerator,
        IFileProvider fileProvider,
        Observable<INotification> notificationListener,
        INamingConventionConverter namingConventionConverter,
        ISyntaxGenerator syntaxGenerator)
    {
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _notificationListener = notificationListener ?? throw new ArgumentNullException(nameof(notificationListener));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _syntaxGenerator = syntaxGenerator ?? throw new ArgumentNullException(nameof(syntaxGenerator));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _namespaceProvider = namespaceProvider ?? throw new ArgumentNullException(nameof(namespaceProvider));
    }

    public void MessageCreate(string name, List<PropertyModel> properties, string directory)
    {
        var classModel = new ClassModel(name);

        classModel.Properties.AddRange(properties);

        classModel.UsingDirectives.Add(new("MediatR"));

        var constructorModel = new ConstructorModel(classModel, classModel.Name);

        foreach (var property in properties)
        {
            classModel.Fields.Add(new()
            {
                Name = $"_{_namingConventionConverter.Convert(NamingConvention.CamelCase, property.Name)}",
                Type = property.Type
            });

            constructorModel.Params.Add(new()
            {
                Name = $"{_namingConventionConverter.Convert(NamingConvention.CamelCase, property.Name)}",
                Type = property.Type
            });
        }

        classModel.Constructors.Add(constructorModel);

        classModel.Implements.Add(new("IRequest"));

        var classFileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, directory, "cs");

        _artifactGenerator.CreateFor(classFileModel);

    }

    public void MessageHandlerCreate(string name, string directory)
    {
        var messageName = $"{name}Message";

        var messageHandlerName = $"{messageName}Handler";

        var classModel = new ClassModel(messageHandlerName);

        classModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = "MediatR" });

        classModel.Fields = new List<FieldModel>()
        {
            FieldModel.LoggerOf(name)
        };

        var constructorModel = new ConstructorModel(classModel, classModel.Name)
        {
            Params = new List<ParamModel>()
            {
                ParamModel.LoggerOf(name)
            }
        };

        foreach (var typeModel in new List<TypeModel>() { })
        {
            classModel.Fields.Add(new FieldModel()
            {
                Name = $"_{_namingConventionConverter.Convert(NamingConvention.CamelCase, _syntaxGenerator.CreateFor(typeModel))}",
                Type = typeModel
            });

            constructorModel.Params.Add(new ParamModel()
            {
                Name = $"{_namingConventionConverter.Convert(NamingConvention.CamelCase, _syntaxGenerator.CreateFor(typeModel))}",
                Type = typeModel
            });
        }

        classModel.Constructors.Add(constructorModel);

        classModel.Implements.Add(new TypeModel("IRequestHandler")
        {
            GenericTypeParameters = new List<TypeModel>
            {
                new TypeModel(messageName)
            }
        });

        var methodModel = new MethodModel()
        {
            ReturnType = TypeModel.Task,
            Async = true,
            Name = "Handle",
            AccessModifier = AccessModifier.Public,
            Params = new List<ParamModel>()
            {
                new ParamModel()
                {
                    Type = new TypeModel(messageName),
                    Name = "message"
                },
                ParamModel.CancellationToken
            },
            Body = new StringBuilder().AppendLine("_logger.LogInformation(\"Message Handled: {message}\", message);").ToString()
        };

        classModel.Methods.Add(methodModel);

        var classFileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, directory, "cs");

        _artifactGenerator.CreateFor(classFileModel);

    }

    public void ServiceBusMessageConsumerCreate(string name = "ServiceBusMessageConsumer", string messagesNamespace = null, string directory = null)
    {

        var classModel = new ClassModel(name);

        if (string.IsNullOrEmpty(messagesNamespace))
        {
            var projectDirectory = Path.GetDirectoryName(_fileProvider.Get("*.csproj", directory));

            var projectNamespace = _namespaceProvider.Get(projectDirectory);

            messagesNamespace = $"{projectNamespace.Split('.').First()}.Core.Messages";
        }

        classModel.Implements.Add(new("BackgroundService"));

        classModel.UsingDirectives.Add(new("Messaging"));

        classModel.UsingDirectives.Add(new("Messaging.Udp"));

        classModel.UsingDirectives.Add(new("Microsoft.Extensions.DependencyInjection"));

        classModel.UsingDirectives.Add(new("Microsoft.Extensions.Hosting"));

        classModel.UsingDirectives.Add(new("System.Text"));

        classModel.UsingDirectives.Add(new("Microsoft.Extensions.Logging"));

        classModel.UsingDirectives.Add(new("System.Threading.Tasks"));

        classModel.UsingDirectives.Add(new("System.Threading"));

        classModel.UsingDirectives.Add(new("MediatR"));

        classModel.UsingDirectives.Add(new("System.Linq"));


        var constructorModel = new ConstructorModel(classModel, classModel.Name);

        foreach (var type in new TypeModel[] { TypeModel.LoggerOf("ServiceBusMessageConsumer"), new TypeModel("IServiceScopeFactory"), new TypeModel("IUdpClientFactory") })
        {
            var propName = type.Name switch
            {
                "ILogger" => "logger",
                "IUdpClientFactory" => "udpClientFactory",
                "IServiceScopeFactory" => "serviceScopeFactory"
            };

            classModel.Fields.Add(new()
            {
                Name = $"_{propName}",
                Type = type
            });

            constructorModel.Params.Add(new()
            {
                Name = propName,
                Type = type
            });
        }

        classModel.Fields.Add(new()
        {
            Name = $"_supportedMessageTypes",
            Type = new("string[]"),
            DefaultValue = "new string[] { }"
        });

        var methodBody = new string[]
        {
            "var client = _udpClientFactory.Create();",

            "",

            "while(!stoppingToken.IsCancellationRequested) {",

            "",

            "var result = await client.ReceiveAsync(stoppingToken);".Indent(1),

            "",

            "var json = Encoding.UTF8.GetString(result.Buffer);".Indent(1),

            "",

            "var message = System.Text.Json.JsonSerializer.Deserialize<ServiceBusMessage>(json)!;".Indent(1),

            "",

            "var messageType = message.MessageAttributes[\"MessageType\"];".Indent(1),

            "",

            "if(_supportedMessageTypes.Contains(messageType))".Indent(1),

            "{".Indent(1),

            new StringBuilder()
            .Append("var type = Type.GetType($\"")
            .Append(messagesNamespace)
            .Append(".{messageType}\");")
            .ToString()
            .Indent(2),

            "",

            "var request = System.Text.Json.JsonSerializer.Deserialize(message.Body, type!)!;".Indent(2),

            "",

            "using (var scope = _serviceScopeFactory.CreateScope())".Indent(2),

            "{".Indent(2),

            "var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();".Indent(3),

            "",

            "await mediator.Send(request, cancellationToken);".Indent(3),

            "}".Indent(2),

            "}".Indent(1),

            "",

            "await Task.Delay(0);".Indent(1),

            "}",
        };

        var method = new MethodModel
        {
            Name = "ExecuteAsync",
            Override = true,
            AccessModifier = AccessModifier.Protected,
            Async = true,
            ReturnType = new("Task"),
            Body = string.Join(Environment.NewLine, methodBody)
        };

        method.Params.Add(ParamModel.CancellationToken);

        classModel.Constructors.Add(constructorModel);

        classModel.Methods.Add(method);

        _artifactGenerator.CreateFor(new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, directory, "cs"));
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
                Type = TypeModel.LoggerOf(name)
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

        _ = createClass(@interface, name, methods, usingDirectives, directory);

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

            _artifactGenerator.CreateFor(interfaceFile);

            return @interface;
        }

        ClassModel createClass(InterfaceModel @interface, string name, List<MethodModel> methods, List<UsingDirectiveModel> usings, string directory)
        {
            var @class = new ClassModel(name);

            var constructors = new List<ConstructorModel>()
            {
                new ConstructorModel(@class, @class.Name)
                {
                    Params = new List<ParamModel>
                    {
                        new ParamModel()
                        {
                            Type = TypeModel.LoggerOf(name),
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

            _artifactGenerator.CreateFor(classFile);

            _notificationListener.Broadcast(new ServiceFileCreated(@interface.Name, @class.Name, directory));

            return @class;
        }
    }
}

