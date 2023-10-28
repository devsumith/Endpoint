// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Folders;
using Endpoint.Core.Artifacts.Folders.Factories;
using Endpoint.Core.Artifacts.Projects;
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
using Endpoint.Core.SystemModels;
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
    private readonly ISystemContextFactory systemContextFactory;
    private readonly IContext context;
    private ISystemContext systemContext;

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
        IApiProjectService apiProjectService,
        ISystemContextFactory systemContextFactory,
        IContext context,
        ISystemContext systemContext)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
        this.solutionService = solutionService ?? throw new ArgumentNullException(nameof(solutionService));
        this.solutionFactory = solutionFactory ?? throw new ArgumentNullException(nameof(solutionFactory));
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
        this.systemContextFactory = systemContextFactory ?? throw new ArgumentNullException(nameof(systemContextFactory));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.systemContext = systemContext ?? throw new ArgumentNullException(nameof(systemContext));
    }

    public async Task Handle(DddAppCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating Domain Driven Design Application", nameof(DddAppCreateRequestHandler));

        systemContext = await systemContextFactory.DddCreateAsync(request.Name, request.AggregateName, request.Directory);

        context.Set(systemContext);

        var solution = await solutionFactory.DddCreateAync(request.Name, request.Directory);

        await artifactGenerator.GenerateAsync(solution);

        ProjectModel infrastructure = solution.Projects.Single(x => x.Name.Contains("Infrastructure"));

        ProjectModel api = solution.Projects.Single(x => x.Name.Contains(".Api"));

        var entity = new ClassModel(request.AggregateName);

        var dbContext = classFactory.CreateDbContext($"{request.Name}DbContext", new List<EntityModel>()
        {
            new EntityModel(entity.Name) { Properties = entity.Properties },
        }, request.Name);

        await artifactGenerator.GenerateAsync(new CodeFileModel<ClassModel>(dbContext, dbContext.Usings, dbContext.Name, fileSystem.Path.Combine(infrastructure.Directory, "Data"), ".cs"));

        await apiProjectService.ControllerCreateAsync(request.AggregateName, false, fileSystem.Path.Combine(api.Directory, "Controllers"));

        commandService.Start($"dotnet ef migrations add {request.Name.Remove("Service")}_Initial", infrastructure.Directory);

        commandService.Start("dotnet ef database update", infrastructure.Directory);

        await CreateDddAppAsync(solution, request.ApplicationName, request.Version, request.Prefix);

        commandService.Start("code .", solution.SolutionDirectory);
    }

    private async Task CreateDddAppAsync(SolutionModel model, string applicationName, string version, string prefix)
    {
        var temporaryAppName = $"{namingConventionConverter.Convert(NamingConvention.SnakeCase, model.Name)}-app";

        await angularService.CreateWorkspace(temporaryAppName, version, applicationName, "application", prefix, model.SrcDirectory, false);

        fileSystem.Directory.Move(Path.Combine(model.SrcDirectory, temporaryAppName), Path.Combine(model.SrcDirectory, $"{model.Name}.App"));
    }
}
