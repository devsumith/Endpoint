// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Cli.Commands;
[Verb("service-bus-client-create")]
public class ServiceBusClientCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('t', "project-type")]
    public string ProjectType { get; set; } = "web";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ServiceBusClientCreateRequestHandler : IRequestHandler<ServiceBusClientCreateRequest>
{
    private readonly ILogger<ServiceBusClientCreateRequestHandler> _logger;
    private readonly ISolutionFactory _solutionFactory;
    private readonly ISolutionService _solutionService;
    private readonly ICommandService _commandService;
    private readonly IProjectFactory _projectFactory;
    private readonly IClassFactory _classFactory;
    private readonly IProjectService _projectService;

    public ServiceBusClientCreateRequestHandler(
        ILogger<ServiceBusClientCreateRequestHandler> logger,
        ISolutionService solutionService,
        ISolutionFactory solutionFactory,
        ICommandService commandService,
        IProjectFactory projectFactory,
        IClassFactory classFactory,
        IProjectService projectService
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _solutionFactory = solutionFactory ?? throw new ArgumentNullException(nameof(solutionFactory));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _projectFactory = projectFactory ?? throw new ArgumentNullException(nameof(projectFactory));
        _classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
    }

    public async Task Handle(ServiceBusClientCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SolutionCreateRequestHandler));

        var model = await _projectFactory.Create(request.ProjectType, request.Name, request.Directory);

        var serviceBusMessageConsumerClassModel = _classFactory.CreateServiceBusMessageConsumer("ServiceBusMessageConsumer", model.Name);

        if (request.ProjectType == "worker")
        {
            var programFileModel = new ContentFileModel(new StringBuilder()
                .AppendLine("var host = Host.CreateDefaultBuilder(args)")
                .AppendLine(".ConfigureServices(services =>".Indent(1))
                .AppendLine("{".Indent(1))
                .AppendLine($"services.Add{model.Name.Split('.').Last()}Services();".Indent(2))
                .AppendLine("})".Indent(1))
                .AppendLine(".Build();".Indent(1))
                .AppendLine(string.Empty)
                .AppendLine("host.Run();")
                .ToString(), "Program", model.Directory, ".cs");

            model.Files.Add(programFileModel);
        }

        var configureServicesClassModel = _classFactory.CreateConfigureServices(model.Name.Split('.').Last());

        var configureServicesMethodBodyBuilder = new StringBuilder();

        if (request.ProjectType == "web")
        {
            configureServicesMethodBodyBuilder
                .AppendLine("services.AddCors(options => options.AddPolicy(\"CorsPolicy\",")
                .AppendLine("builder => builder".Indent(1))
                .AppendLine(".WithOrigins(\"http://localhost:4200\")".Indent(1))
                .AppendLine(".AllowAnyMethod()".Indent(1))
                .AppendLine(".AllowAnyHeader()".Indent(1))
                .AppendLine(".SetIsOriginAllowed(isOriginAllowed: _ => true)".Indent(1))
                .AppendLine(".AllowCredentials()));".Indent(1))
                .AppendLine("services.AddControllers();")
                .AppendLine("services.AddEndpointsApiExplorer();")
                .AppendLine("services.AddSwaggerGen();");
        }

        configureServicesMethodBodyBuilder.AppendLine("services.AddMessagingUdpServices();")
            .AppendLine("services.AddHostedService<ServiceBusMessageConsumer>();");

        configureServicesClassModel.Methods.First().Body = new Core.Syntax.Expressions.ExpressionModel(configureServicesMethodBodyBuilder.ToString());

        var configureServicesFileModel = new ObjectFileModel<ClassModel>(configureServicesClassModel, new()
        {
            new (model.Name)

        }, configureServicesClassModel.Name, model.Directory, ".cs");

        configureServicesFileModel.Namespace = "Microsoft.Extensions.DependencyInjection";

        model.Files.Add(configureServicesFileModel);

        model.Files.Add(new ObjectFileModel<ClassModel>(serviceBusMessageConsumerClassModel, serviceBusMessageConsumerClassModel.UsingDirectives, serviceBusMessageConsumerClassModel.Name, model.Directory, ".cs"));

        model.References.Add(@"..\Messaging.Udp\Messaging.Udp.csproj");

        _projectService.AddProjectAsync(model);

        if (File.Exists(Path.Combine(model.Directory, "Worker.cs")))
        {
            File.Delete(Path.Combine(model.Directory, "Worker.cs"));
        }
    }
}
