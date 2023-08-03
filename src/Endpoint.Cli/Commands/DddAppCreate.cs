// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Folders.Factories;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Folders;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Entities.Aggregate;
using Endpoint.Core.Syntax.Entities;
using Endpoint.Core.Artifacts.Services;

namespace Endpoint.Cli.Commands;

[Verb("ddd-app-create")]
public class DddAppCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

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
    private readonly ILogger<DddAppCreateRequestHandler> _logger;
    private readonly ISolutionService _solutionService;
    private readonly IAngularService _angularService;
    private readonly ISolutionModelFactory _solutionModelFactory;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ICommandService _commandService;
    private readonly IClassModelFactory _classModelFactory;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IFolderFactory _folderFactory;
    private readonly IProjectFactory _projectModelFactory;
    private readonly IAggregateService _aggregateService;
    private readonly IApiProjectService _apiProjectService;

    public DddAppCreateRequestHandler(
        ILogger<DddAppCreateRequestHandler> logger,
        ISolutionService solutionService,
        IAngularService angularService,
        ISolutionModelFactory solutionModelFactory,
        IArtifactGenerator artifactGenerator,
        INamingConventionConverter namingConventionConverter,
        ICommandService commandService,
        IClassModelFactory classModelFactory,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IFileModelFactory fileModelFactory,
        IFolderFactory folderFactory,
        IProjectFactory projectModelFactory,
        IAggregateService aggregateService,
        IApiProjectService apiProjectService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _solutionModelFactory = solutionModelFactory;
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter)); ;
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _folderFactory = folderFactory ?? throw new ArgumentNullException(nameof(folderFactory));
        _projectModelFactory = projectModelFactory ?? throw new ArgumentNullException(nameof(projectModelFactory));
        _aggregateService = aggregateService ?? throw new ArgumentNullException(nameof(aggregateService));
        _apiProjectService = apiProjectService ?? throw new ArgumentNullException(nameof(apiProjectService));
    }

    public async Task Handle(DddAppCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(DddAppCreateRequestHandler));

        var solution = await CreateDddSolution(request.Name, request.AggregateName, request.Properties, request.Directory);

        await CreateDddApp(solution, request.ApplicationName, request.Version, request.Prefix);

        _commandService.Start("code .", solution.SolutionDirectory);
    }

    private async Task<SolutionModel> CreateDddSolution(string name, string aggregateName, string properties, string directory)
    {
        var model = new SolutionModel(name, directory);

        var schema = name.Remove("Service");

        var sourceFolder = new FolderModel("src", model.SolutionDirectory);

        var buildingBlocksFolder = new FolderModel("BuildingBlocks", sourceFolder.Directory) { Priority = 1 };

        var kernel = await _projectModelFactory.CreateKernelProject(buildingBlocksFolder.Directory);

        var messaging = await _projectModelFactory.CreateMessagingProject(buildingBlocksFolder.Directory);

        var messagingUdp = await _projectModelFactory.CreateMessagingUdpProject(buildingBlocksFolder.Directory);

        var security = await _projectModelFactory.CreateSecurityProject(buildingBlocksFolder.Directory);

        var validation = await _projectModelFactory.CreateValidationProject(buildingBlocksFolder.Directory);

        buildingBlocksFolder.Projects.Add(messaging);

        buildingBlocksFolder.Projects.Add(messagingUdp);

        buildingBlocksFolder.Projects.Add(security);

        buildingBlocksFolder.Projects.Add(kernel);

        buildingBlocksFolder.Projects.Add(validation);

        var core = await _projectModelFactory.CreateCore(name, sourceFolder.Directory);

        var infrastructure = await _projectModelFactory.CreateInfrastructure(name, sourceFolder.Directory);

        var api = await _projectModelFactory.CreateApi(name, sourceFolder.Directory);

        sourceFolder.Projects.AddRange(new[] { core, infrastructure, api });

        model.Folders.Add(sourceFolder);

        model.Folders.Add(buildingBlocksFolder);

        model.Folders = model.Folders.OrderByDescending(x => x.Priority).ToList();

        await _artifactGenerator.CreateAsync(model);

        var aggregatesModelDirectory = Path.Combine(core.Directory, "AggregatesModel");

        var entity = await _aggregateService.Add(aggregateName, properties, aggregatesModelDirectory, name);

        var dbContext = _classModelFactory.CreateDbContext($"{name}DbContext", new List<EntityModel>()
        {
            new EntityModel(entity.Name) { Properties = entity.Properties}
        }, name);

        await _artifactGenerator.CreateAsync(new ObjectFileModel<ClassModel>(dbContext, dbContext.UsingDirectives, dbContext.Name, Path.Combine(infrastructure.Directory, "Data"), "cs"));

        await _apiProjectService.ControllerAdd(aggregateName, false, Path.Combine(api.Directory, "Controllers"));

        _commandService.Start($"dotnet ef migrations add {schema}_Initial", infrastructure.Directory);

        _commandService.Start("dotnet ef database update", infrastructure.Directory);

        return model;
    }

    private async Task CreateDddApp(SolutionModel model, string applicationName, string version, string prefix)
    {
        var temporaryAppName = $"{_namingConventionConverter.Convert(NamingConvention.SnakeCase, model.Name)}-app";

        await _angularService.CreateWorkspace(temporaryAppName, version, applicationName, "application", prefix, model.SrcDirectory, false);

        Directory.Move(Path.Combine(model.SrcDirectory, temporaryAppName), Path.Combine(model.SrcDirectory, $"{model.Name}.App"));
    }
}
