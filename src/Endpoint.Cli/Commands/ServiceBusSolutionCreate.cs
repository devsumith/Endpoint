// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Services;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Models.Artifacts.Projects.Factories;
using System.Linq;
using Endpoint.Core.Models.Artifacts.Folders;
using Endpoint.Core.Models.Syntax.Classes.Factories;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax.Classes;
using System.Collections.Generic;
using System.Text;

namespace Endpoint.Cli.Commands;


[Verb("service-bus-solution-create")]
public class ServiceBusSolutionCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }

    [Option('p')]
    public string ProjectName { get; set; }

    [Option('t')]
    public string ProjectType { get; set; } = "worker";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ServiceBusSolutionCreateRequestHandler : IRequestHandler<ServiceBusSolutionCreateRequest>
{
    private readonly ILogger<ServiceBusSolutionCreateRequestHandler> _logger;
    private readonly ISolutionModelFactory _solutionModelFactory;
    private readonly ISolutionService _solutionService;
    private readonly ICommandService _commandService;
    private readonly IProjectModelFactory _projectModelFactory;
    private readonly IClassModelFactory _classModelFactory;
    public ServiceBusSolutionCreateRequestHandler(
        ILogger<ServiceBusSolutionCreateRequestHandler> logger,
        ISolutionService solutionService,
        ISolutionModelFactory solutionModelFactory,
        ICommandService commandService,
        IProjectModelFactory projectModelFactory,
        IClassModelFactory classModelFactory
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _solutionModelFactory = solutionModelFactory ?? throw new ArgumentNullException(nameof(solutionModelFactory));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _projectModelFactory = projectModelFactory ?? throw new ArgumentNullException(nameof(projectModelFactory));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));
    }

    public async Task Handle(ServiceBusSolutionCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SolutionCreateRequestHandler));

        if(string.IsNullOrEmpty(request.ProjectName))
        {
            request.ProjectName = $"{request.Name}";
        }

        var model = _solutionModelFactory.Create(request.Name, request.ProjectName, request.ProjectType, string.Empty, request.Directory);
        
        var srcFolder = model.Folders.Single();

        var projectModel = srcFolder.Projects.Single();

        projectModel.Order = int.MaxValue;

        var serviceBusMessageConsumerClassModel = _classModelFactory.CreateServiceBusMessageConsumer("ServiceBusMessageConsumer", projectModel.Name);

        var configureServicesClassModel = _classModelFactory.CreateConfigureServices(projectModel.Name.Split('.').Last());

        configureServicesClassModel.Methods.First().Body = new StringBuilder()
            .AppendLine("services.AddMessagingUdpServices();")
            .AppendLine("services.AddHostedService<ServiceBusMessageConsumer>();")
            .ToString();

        projectModel.Files.Add(new ObjectFileModel<ClassModel>(serviceBusMessageConsumerClassModel, serviceBusMessageConsumerClassModel.UsingDirectives, serviceBusMessageConsumerClassModel.Name, projectModel.Directory, "cs"));

        var configureServicesFileModel = new ObjectFileModel<ClassModel>(configureServicesClassModel, new ()
        {
            new (projectModel.Name)

        }, configureServicesClassModel.Name, projectModel.Directory, "cs");

        configureServicesFileModel.Namespace = "Microsoft.Extensions.DependencyInjection";

        projectModel.Files.Add(configureServicesFileModel);

        projectModel.References.Add(@"..\Messaging.Udp\Messaging.Udp.csproj");

        srcFolder.Projects.Add(_projectModelFactory.CreateMessagingProject(srcFolder.Directory));

        srcFolder.Projects.Add(_projectModelFactory.CreateMessagingUdpProject(srcFolder.Directory));

        _solutionService.Create(model);

        _commandService.Start($"start {model.SolultionFileName}", model.SolutionDirectory);

    }
}
