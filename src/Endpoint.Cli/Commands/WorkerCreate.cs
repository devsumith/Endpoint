// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Internals;
using Endpoint.Core.Messages;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Constructors;
using Endpoint.Core.Syntax.Fields;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax.Params;
using Endpoint.Core.Syntax.Types;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("worker-create")]
public class WorkerCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class WorkerCreateRequestHandler : IRequestHandler<WorkerCreateRequest>
{
    private readonly ILogger<WorkerCreateRequestHandler> _logger;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly Observable<INotification> _notificationListener;

    public WorkerCreateRequestHandler(
        ILogger<WorkerCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator,
        Observable<INotification> notificationListener)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _notificationListener = notificationListener ?? throw new ArgumentNullException(nameof(notificationListener));
    }

    public async Task Handle(WorkerCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(WorkerCreateRequestHandler));

        var usings = new List<UsingDirectiveModel>()
        {
            new UsingDirectiveModel() { Name = "Microsoft.Extensions.Hosting" },
            new UsingDirectiveModel() { Name = "Microsoft.Extensions.Logging" },
            new UsingDirectiveModel() { Name = "System" },
            new UsingDirectiveModel() { Name = "System.Threading" },
            new UsingDirectiveModel() { Name = "System.Threading.Tasks" }
        };

        var model = new ClassModel(request.Name);

        var fields = new List<FieldModel>()
        {
            new FieldModel()
            {
                Name = "_logger",
                Type = TypeModel.LoggerOf(request.Name)
            }
        };

        var constructors = new List<ConstructorModel>()
        {

            new ConstructorModel(model,model.Name)
            {
                Params = new List<ParamModel>
                {
                    new ParamModel()
                    {
                        Type = TypeModel.LoggerOf(request.Name),
                        Name = "logger"
                    }
                }
            }
        };

        model.Fields = fields;

        model.Constructors = constructors;

        model.Implements.Add(new TypeModel() { Name = "BackgroundService" });

        var methodBodyBuilder = new StringBuilder();

        methodBodyBuilder.AppendLine("while (!stoppingToken.IsCancellationRequested)");

        methodBodyBuilder.AppendLine("{");

        methodBodyBuilder.AppendLine("_logger.LogInformation(\"Worker running at: {time}\", DateTimeOffset.Now);".Indent(1));

        methodBodyBuilder.AppendLine("await Task.Delay(1000, stoppingToken);".Indent(1));

        methodBodyBuilder.AppendLine("}");

        var method = new MethodModel()
        {
            Name = "ExecuteAsync",
            Override = true,
            AccessModifier = AccessModifier.Protected,
            Body = methodBodyBuilder.ToString(),
            Async = true,
            ReturnType = new TypeModel() { Name = "Task" },
            Params = new List<ParamModel>
            {
                new ParamModel()
                {
                    Name = "stoppingToken",
                    Type = new TypeModel() { Name = "CancellationToken" }
                }
            }
        };

        model.Methods.Add(method);

        var fileModel = new ObjectFileModel<ClassModel>(model, usings, model.Name, request.Directory, "cs");

        _artifactGenerator.CreateFor(fileModel);

        _notificationListener.Broadcast(new WorkerFileCreated(model.Name, request.Directory));
    }
}
