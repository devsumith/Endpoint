// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.Artifacts.Folders;
using Endpoint.Core.Models.Artifacts.Folders.Factories;
using Endpoint.Core.Models.Artifacts.Projects.Factories;
using Endpoint.Core.Models.Artifacts.Projects.Services;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Classes.Factories;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.Entities.Aggregate;
using Endpoint.Core.Models.WebArtifacts.Services;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ICommandService _commandService;
    private readonly IClassModelFactory _classModelFactory;
    private readonly IFileProvider _fileProvider;
    private readonly IFileSystem _fileSystem;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IFolderFactory _folderFactory;
    private readonly IProjectModelFactory _projectModelFactory;
    private readonly IAggregateService _aggregateService;
    private readonly IApiProjectService _apiProjectService;

    public DddAppCreateRequestHandler(
        ILogger<DddAppCreateRequestHandler> logger,
        ISolutionService solutionService,
        IAngularService angularService,
        ISolutionModelFactory solutionModelFactory,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        INamingConventionConverter namingConventionConverter,
        ICommandService commandService,
        IClassModelFactory classModelFactory,
        IFileProvider fileProvider,
        IFileSystem fileSystem,
        IFileModelFactory fileModelFactory,
        IFolderFactory folderFactory,
        IProjectModelFactory projectModelFactory,
        IAggregateService aggregateService,
        IApiProjectService apiProjectService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
        _solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        _solutionModelFactory = solutionModelFactory;
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
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

        CreateDddApp(solution, request.ApplicationName, request.Version, request.Prefix);

        _commandService.Start("code .", solution.SolutionDirectory);
    }

    private async Task<SolutionModel> CreateDddSolution(string name, string aggregateName, string properties, string directory)
    {
        var model = new SolutionModel(name, directory);

        var sourceFolder = new FolderModel("src", model.SolutionDirectory);

        var buildingBlocksFolder = new FolderModel("BuildingBlocks", sourceFolder.Directory);

        var kernal = _projectModelFactory.CreateKernelProject(buildingBlocksFolder.Directory);

        var messaging = _projectModelFactory.CreateMessagingProject(buildingBlocksFolder.Directory);

        var messagingUdp = _projectModelFactory.CreateMessagingUdpProject(buildingBlocksFolder.Directory);

        var security = _projectModelFactory.CreateSecurityProject(buildingBlocksFolder.Directory);

        var validation = _projectModelFactory.CreateValidationProject(buildingBlocksFolder.Directory);

        buildingBlocksFolder.Projects.Add(messaging);

        buildingBlocksFolder.Projects.Add(messagingUdp);

        buildingBlocksFolder.Projects.Add(security);

        buildingBlocksFolder.Projects.Add(kernal);

        buildingBlocksFolder.Projects.Add(validation);

        var core = _projectModelFactory.CreateCore(name, sourceFolder.Directory);

        var infrastructure = _projectModelFactory.CreateInfrastructure(name, sourceFolder.Directory);

        var api = _projectModelFactory.CreateApi(name, sourceFolder.Directory);

        sourceFolder.Projects.AddRange(new[] { core, infrastructure, api });

        model.Folders.Add(sourceFolder);

        model.Folders.Add(buildingBlocksFolder);

        _artifactGenerationStrategyFactory.CreateFor(model);

        var aggregateModelDirectory = Path.Combine(core.Directory, "AggregateModel");

        var entity = await _aggregateService.Add(aggregateName, properties, aggregateModelDirectory, name);

        var dbContext = _classModelFactory.CreateDbContext($"{name}DbContext", new List<EntityModel>()
        {
            new EntityModel(entity.Name) { Properties = entity.Properties}
        }, name);

        _artifactGenerationStrategyFactory.CreateFor(new ObjectFileModel<ClassModel>(dbContext, dbContext.UsingDirectives, dbContext.Name, Path.Combine(infrastructure.Directory, "Data"), "cs"));

        _apiProjectService.ControllerAdd(aggregateName, Path.Combine(api.Directory, "Controllers"));

        _commandService.Start("dotnet ef migrations add Initial", infrastructure.Directory);

        _commandService.Start("dotnet ef database update", infrastructure.Directory);

        return model;
    }

    private void CreateDddApp(SolutionModel model, string applicationName, string version, string prefix)
    {
        var temporaryAppName = $"{_namingConventionConverter.Convert(NamingConvention.SnakeCase, model.Name)}-app";

        _angularService.CreateWorkspace(temporaryAppName, version, applicationName, "application", prefix, model.SrcDirectory, false);

        Directory.Move(Path.Combine(model.SrcDirectory, temporaryAppName), Path.Combine(model.SrcDirectory, $"{model.Name}.App"));
    }
}
