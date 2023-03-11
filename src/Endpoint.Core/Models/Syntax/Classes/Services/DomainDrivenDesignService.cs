// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Types;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Models.Syntax.Classes.Services;

public class DomainDrivenDesignService : IDomainDrivenDesignService
{
    private readonly ILogger<DomainDrivenDesignService> _logger;

    public DomainDrivenDesignService(ILogger<DomainDrivenDesignService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ClassModel ServiceBusMessageConsumerCreate(string messagesNamespace, string directory)
    {
        var classModel = new ClassModel("ServiceBusMessageConsumer");

        classModel.Implements.Add(new TypeModel("BackgroundService"));

        classModel.UsingDirectives.Add(new ("MediatR"));

        classModel.UsingDirectives.Add(new ("Messaging"));

        classModel.UsingDirectives.Add(new ("Newtonsoft.Json"));

        classModel.UsingDirectives.Add(new ("Microsoft.Extensions.Hosting"));

        classModel.UsingDirectives.Add(new ("Microsoft.Extensions.Logging"));

        var ctor = new ConstructorModel(classModel, classModel.Name);

        foreach (var type in new TypeModel[] { TypeModel.LoggerOf("ServiceBusMessageConsumer"), new TypeModel("IMediator"), new TypeModel("IMessagingClient") })
        {
            var propName = type.Name switch
            {
                "ILogger" => "logger",
                "IMessagingClient" => "messagingClient",
                "IMediator" => "mediator"
            };

            classModel.Fields.Add(new FieldModel()
            {
                Name = $"_{propName}",
                Type = type
            });

            ctor.Params.Add(new ParamModel()
            {
                Name = propName,
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
            ($"var type = Type.GetType($\"{messagesNamespace}." + "{messageType}\");").Indent(2),
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

        return classModel;

    }

}


