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
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("worker-create")]
public class WorkerCreateRequest : IRequest<Unit> {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class WorkerCreateRequestHandler : IRequestHandler<WorkerCreateRequest, Unit>
{
    private readonly ILogger<WorkerCreateRequestHandler> _logger;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly IDependencyInjectionService _dependencyInjectionService;
    private readonly Observable<INotification> _observableNotifications;


    public WorkerCreateRequestHandler(
        ILogger<WorkerCreateRequestHandler> logger,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        IDependencyInjectionService dependencyInjectionService,
        Observable<INotification> observableNotifications)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _dependencyInjectionService = dependencyInjectionService ?? throw new ArgumentNullException(nameof(dependencyInjectionService));
        _observableNotifications = observableNotifications ?? throw new ArgumentNullException(nameof(observableNotifications));
        
    }

    public async Task<Unit> Handle(WorkerCreateRequest request, CancellationToken cancellationToken)
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
                Type = new TypeModel()
                {
                    Name = "ILogger",
                    GenericTypeParameters = new List<TypeModel>()
                    {
                        new TypeModel() { Name = request.Name }
                    }
                }
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
                        Type = new TypeModel()
                        {
                            Name = "ILogger",
                            GenericTypeParameters = new List<TypeModel>()
                            {
                                new TypeModel() { Name = request.Name}
                            }
                        },
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

        _artifactGenerationStrategyFactory.CreateFor(fileModel);


        _observableNotifications.Broadcast(new WorkerFileCreated());

        _dependencyInjectionService.AddHosted(model.Name, request.Directory);

        return new();
    }
}