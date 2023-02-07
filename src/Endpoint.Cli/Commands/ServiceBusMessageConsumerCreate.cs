using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Enums;
using Endpoint.Core.Internals;
using Endpoint.Core.Messages;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("service-bus-message-consumer-create")]
public class ServiceBusMessageConsumerCreateRequest : IRequest<Unit> {
    [Option('n')]
    public string Name { get; set; } = "ServiceBusMessageConsumer";

    [Option('m')]
    public string MessagesNamespace { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ServiceBusMessageConsumerCreateRequestHandler : IRequestHandler<ServiceBusMessageConsumerCreateRequest, Unit>
{
    private readonly ILogger<ServiceBusMessageConsumerCreateRequestHandler> _logger;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly Observable<INotification> _notificationListener;
    private readonly IFileProvider _fileProvider;
    private readonly INamespaceProvider _namespaceProvider;
    public ServiceBusMessageConsumerCreateRequestHandler(
        ILogger<ServiceBusMessageConsumerCreateRequestHandler> logger,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        Observable<INotification> notificationListener,
        IFileProvider fileProvider,
        INamespaceProvider namespaceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _notificationListener = notificationListener ?? throw new ArgumentNullException(nameof(notificationListener));
        _namespaceProvider = namespaceProvider;
        _fileProvider = fileProvider;
    }

    public async Task<Unit> Handle(ServiceBusMessageConsumerCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ServiceBusMessageConsumerCreateRequestHandler));

        var classModel = new ClassModel(request.Name);

        if(string.IsNullOrEmpty(request.MessagesNamespace))
        {
            var projectDirectory = Path.GetDirectoryName(_fileProvider.Get("*.csproj", request.Directory));

            var projectNamespace = _namespaceProvider.Get(projectDirectory);

            request.MessagesNamespace = $"{projectNamespace.Split('.').First()}.Core.Messages";
        }

        classModel.Implements.Add(new TypeModel("BackgroundService"));

        classModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = "MediatR" });

        classModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = "Messaging" });

        classModel.UsingDirectives.Add(new UsingDirectiveModel() { Name = "Newtonsoft.Json" });

        var ctor = new ConstructorModel(classModel, classModel.Name);

        foreach(var type in new TypeModel[] { TypeModel.LoggerOf("ServiceBusMessageConsumer"), new TypeModel("IMediator"), new TypeModel("IMessagingClient") })
        {
            var name = type.Name switch
            {
                "ILogger" => "logger",
                "IMessagingClient" => "messagingClient",
                "IMediator" => "mediator"
            };

            classModel.Fields.Add(new FieldModel()
            {
                Name = $"_{name}",
                Type = type
            });

            ctor.Params.Add(new ParamModel()
            {
                Name = name,
                Type = type
            });
        }

        var methodBody = new string[]
        {
            "await _messagingClient.StartAsync(stoppingToken);",
            "",
            "while(!stoppingToken.IsCancellationRequested) {",
            "",
            "try".Indent(1),
            "{".Indent(1),
            "var message = await _messagingClient.ReceiveAsync(new ReceiveRequest());".Indent(2),
            "",
            "var messageType = message.MessageAttributes[\"MessageType\"];".Indent(2),
            "",
            ($"var type = Type.GetType($\"{request.MessagesNamespace}." + "{messageType}\");").Indent(2),
            "",
            "var request = JsonConvert.DeserializeObject(message.Body, type!) as IRequest;".Indent(2),
            "",
            "await _mediator.Send(request!);".Indent(2),
            "",
            "await Task.Delay(100);".Indent(2),
            "}".Indent(1),
            "catch(Exception exception)".Indent(1),
            "{".Indent(1),
            "_logger.LogError(exception.Message);".Indent(2),
            "",
            "continue;".Indent(2),
            "}".Indent(1),
            "}"
        };

        var method = new MethodModel
        {
            Name = "ExecuteAsync",
            Override = true,
            AccessModifier = AccessModifier.Protected,
            Async = true,
            ReturnType = new TypeModel("Task"),
            Body = string.Join(Environment.NewLine, methodBody)
        };

        method.Params.Add(new ParamModel()
        {
            Name = "stoppingToken",
            Type = new TypeModel("CancellationToken")
        });

        classModel.Constructors.Add(ctor);

        classModel.Methods.Add(method);

        _artifactGenerationStrategyFactory.CreateFor(new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, request.Directory, "cs"));

        _notificationListener.Broadcast(new WorkerFileCreated(classModel.Name, request.Directory));

        return new();
    }
}