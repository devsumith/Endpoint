// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Folders;
using Endpoint.Core.Artifacts.Folders.Factories;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Artifacts.Services;
using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Artifacts.Solutions.Factories;
using Endpoint.Core.Artifacts.Solutions.Services;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Entities;
using Endpoint.Core.Syntax.Units.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("ddd-app-create")]
public class DddAppCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; } = "Microservices";

    [Option('a', "aggregate")]
    public string AggregateName { get; set; } = "ToDo";

    [Option("properties")]
    public string Properties { get; set; } = "Title:string,IsComplete:bool";

    [Option("app-name")]
    public string ApplicationName { get; set; } = "app";

    [Option('v', "version")]
    public string Version { get; set; } = "latest";

    [Option('p', "prefix")]
    public string Prefix { get; set; } = "app";

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class DddAppCreateRequestHandler : IRequestHandler<DddAppCreateRequest>
{
    private readonly ILogger<DddAppCreateRequestHandler> logger;
    private readonly ISolutionService solutionService;
    private readonly IAngularService angularService;
    private readonly ISolutionFactory solutionFactory;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly INamingConventionConverter namingConventionConverter;
    private readonly ICommandService commandService;
    private readonly IClassFactory classFactory;
    private readonly IFileProvider fileProvider;
    private readonly IFileSystem fileSystem;
    private readonly IFileFactory fileFactory;
    private readonly IFolderFactory folderFactory;
    private readonly IProjectFactory projectFactory;
    private readonly IAggregateService aggregateService;
    private readonly IApiProjectService apiProjectService;

    public DddAppCreateRequestHandler(
        ILogger<DddAppCreateRequestHandler> logger,
        ISolutionService solutionService,
        IAngularService angularService,
        ISolutionFactory solutionFactory,
        IArtifactGenerator artifactGenerator,
        INamingConventionConverter namingConventionConverter,
        ICommandService commandService,
        IClassFactory classFactory,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IFileFactory fileFactory,
        IFolderFactory folderFactory,
        IProjectFactory projectFactory,
        IAggregateService aggregateService,
        IApiProjectService apiProjectService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
        this.solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        this.solutionFactory = solutionFactory;
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        this.commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        this.classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        this.folderFactory = folderFactory ?? throw new ArgumentNullException(nameof(folderFactory));
        this.projectFactory = projectFactory ?? throw new ArgumentNullException(nameof(projectFactory));
        this.aggregateService = aggregateService ?? throw new ArgumentNullException(nameof(aggregateService));
        this.apiProjectService = apiProjectService ?? throw new ArgumentNullException(nameof(apiProjectService));
    }

    public async Task Handle(DddAppCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(DddAppCreateRequestHandler));

        var solution = await CreateDddSolution(request.Name, request.AggregateName, request.Properties, request.Directory);

        await CreateDddApp(solution, request.ApplicationName, request.Version, request.Prefix);

        commandService.Start("code .", solution.SolutionDirectory);
    }

    private async Task<SolutionModel> CreateDddSolution(string name, string aggregateName, string properties, string directory)
    {
        var model = new SolutionModel(name, directory);

        var schema = name.Remove("Service");

        var sourceFolder = new FolderModel("src", model.SolutionDirectory);

        var servicesFolder = new FolderModel("Services", sourceFolder.Directory);

        var serviceFolder = new FolderModel(schema, servicesFolder.Directory);

        var buildingBlocksFolder = new FolderModel("BuildingBlocks", sourceFolder.Directory) { Priority = 1 };

        var kernel = await projectFactory.CreateKernelProject(buildingBlocksFolder.Directory);

        var messaging = await projectFactory.CreateMessagingProject(buildingBlocksFolder.Directory);

        var messagingUdp = await projectFactory.CreateMessagingUdpProject(buildingBlocksFolder.Directory);

        var security = await projectFactory.CreateSecurityProject(buildingBlocksFolder.Directory);

        var validation = await projectFactory.CreateValidationProject(buildingBlocksFolder.Directory);

        var compression = await projectFactory.CreateIOCompression(buildingBlocksFolder.Directory);

        buildingBlocksFolder.Projects.Add(messaging);

        buildingBlocksFolder.Projects.Add(messagingUdp);

        buildingBlocksFolder.Projects.Add(security);

        buildingBlocksFolder.Projects.Add(kernel);

        buildingBlocksFolder.Projects.Add(validation);

        buildingBlocksFolder.Projects.Add(compression);

        servicesFolder.Projects.Add(await projectFactory.CreateCommon(servicesFolder.Directory));

        var core = await projectFactory.CreateCore(name, serviceFolder.Directory);

        var infrastructure = await projectFactory.CreateInfrastructure(name, serviceFolder.Directory);

        var api = await projectFactory.CreateApi(name, serviceFolder.Directory);

        sourceFolder.Projects.AddRange(new[] { core, infrastructure, api });

        model.Folders.Add(sourceFolder);

        model.Folders.Add(buildingBlocksFolder);

        model.Folders.Add(servicesFolder);

        model.Folders = model.Folders.OrderByDescending(x => x.Priority).ToList();

        await artifactGenerator.GenerateAsync(model);

        var aggregatesModelDirectory = Path.Combine(core.Directory, "AggregatesModel");

        var entity = await aggregateService.Add(aggregateName, properties, aggregatesModelDirectory, name);

        var dbContext = classFactory.CreateDbContext($"{name}DbContext", new List<EntityModel>()
        {
            new EntityModel(entity.Name) { Properties = entity.Properties },
        }, name);

        await artifactGenerator.GenerateAsync(new CodeFileModel<ClassModel>(dbContext, dbContext.Usings, dbContext.Name, Path.Combine(infrastructure.Directory, "Data"), ".cs"));

        await apiProjectService.ControllerCreateAsync(aggregateName, false, Path.Combine(api.Directory, "Controllers"));

        commandService.Start($"dotnet ef migrations add {schema}_Initial", infrastructure.Directory);

        commandService.Start("dotnet ef database update", infrastructure.Directory);

        return model;
    }

    private async Task CreateDddApp(SolutionModel model, string applicationName, string version, string prefix)
    {
        var temporaryAppName = $"{namingConventionConverter.Convert(NamingConvention.SnakeCase, model.Name)}-app";

        await angularService.CreateWorkspace(temporaryAppName, version, applicationName, "application", prefix, model.SrcDirectory, false);

        Directory.Move(Path.Combine(model.SrcDirectory, temporaryAppName), Path.Combine(model.SrcDirectory, $"{model.Name}.App"));
    }
}
