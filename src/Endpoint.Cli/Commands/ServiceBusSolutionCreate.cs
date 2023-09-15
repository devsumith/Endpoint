// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts.Projects.Factories;
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
using Endpoint.Core.Artifacts.Solutions.Factories;
using Endpoint.Core.Artifacts.Solutions.Services;

namespace Endpoint.Cli.Commands;


[Verb("service-bus-solution-create")]
public class ServiceBusSolutionCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('p')]
    public string ProjectName { get; set; }

    [Option('t')]
    public string ProjectType { get; set; } = "web";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ServiceBusSolutionCreateRequestHandler : IRequestHandler<ServiceBusSolutionCreateRequest>
{
    private readonly ILogger<ServiceBusSolutionCreateRequestHandler> _logger;
    private readonly ISolutionFactory _solutionFactory;
    private readonly ISolutionService _solutionService;
    private readonly ICommandService _commandService;
    private readonly IProjectFactory _projectFactory;
    private readonly IClassFactory _classFactory;
    public ServiceBusSolutionCreateRequestHandler(
        ILogger<ServiceBusSolutionCreateRequestHandler> logger,
        ISolutionService solutionService,
        ISolutionFactory solutionFactory,
        ICommandService commandService,
        IProjectFactory projectFactory,
        IClassFactory classFactory
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _solutionFactory = solutionFactory ?? throw new ArgumentNullException(nameof(solutionFactory));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _projectFactory = projectFactory ?? throw new ArgumentNullException(nameof(projectFactory));
        _classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
    }

    public async Task Handle(ServiceBusSolutionCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SolutionCreateRequestHandler));

        if (string.IsNullOrEmpty(request.ProjectName))
        {
            request.ProjectName = $"{request.Name}";
        }

        var model = await _solutionFactory.Create(request.Name, request.ProjectName, request.ProjectType, string.Empty, request.Directory);

        var srcFolder = model.Folders.Single();

        var projectModel = srcFolder.Projects.Single();

        if (projectModel.Files.Any(x => x.Name == "ConfigureServices"))
        {
            projectModel.Files.Remove(projectModel.Files.Single(x => x.Name == "ConfigureServices"));
        }

        projectModel.Order = int.MaxValue;

        var serviceBusMessageConsumerClassModel = _classFactory.CreateServiceBusMessageConsumer("ServiceBusMessageConsumer", projectModel.Name);

        var configureServicesClassModel = _classFactory.CreateConfigureServices(projectModel.Name.Split('.').Last());

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

        configureServicesMethodBodyBuilder
            .AppendLine("services.AddMessagingUdpServices();")
            .AppendLine("services.AddHostedService<ServiceBusMessageConsumer>();");

        configureServicesClassModel.Methods.First().Body = new Core.Syntax.Expressions.ExpressionModel(configureServicesMethodBodyBuilder.ToString());

        var configureServicesFileModel = new CodeFileModel<ClassModel>(configureServicesClassModel, new()
        {
            new (projectModel.Name)

        }, configureServicesClassModel.Name, projectModel.Directory, ".cs");

        configureServicesFileModel.Namespace = "Microsoft.Extensions.DependencyInjection";

        projectModel.Files.Add(configureServicesFileModel);

        if (request.ProjectType == "worker")
        {
            var programFileModel = new ContentFileModel(new StringBuilder()
                .AppendLine("var host = Host.CreateDefaultBuilder(args)")
                .AppendLine(".ConfigureServices(services =>".Indent(1))
                .AppendLine("{".Indent(1))
                .AppendLine($"services.Add{projectModel.Name.Split('.').Last()}Services();".Indent(2))
                .AppendLine("})".Indent(1))
                .AppendLine(".Build();".Indent(1))
                .AppendLine(string.Empty)
                .AppendLine("host.Run();")
                .ToString(), "Program", projectModel.Directory, ".cs");

            projectModel.Files.Add(programFileModel);
        }

        projectModel.Files.Add(new CodeFileModel<ClassModel>(serviceBusMessageConsumerClassModel, serviceBusMessageConsumerClassModel.Usings, serviceBusMessageConsumerClassModel.Name, projectModel.Directory, ".cs"));

        projectModel.References.Add(@"..\Messaging.Udp\Messaging.Udp.csproj");

        srcFolder.Projects.Add(await _projectFactory.CreateMessagingProject(srcFolder.Directory));

        srcFolder.Projects.Add(await _projectFactory.CreateMessagingUdpProject(srcFolder.Directory));

        await _solutionService.Create(model);

        if (File.Exists(Path.Combine(projectModel.Directory, "Worker.cs")))
        {
            File.Delete(Path.Combine(projectModel.Directory, "Worker.cs"));
        }

        _commandService.Start($"start {model.SolultionFileName}", model.SolutionDirectory);

    }
}
