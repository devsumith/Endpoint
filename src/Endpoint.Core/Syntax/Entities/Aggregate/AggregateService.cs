// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.AggregateModels;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Cqrs;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using static Endpoint.Core.Constants.FileExtensions;

namespace Endpoint.Core.Syntax.Entities.Aggregate;

public class AggregateService : IAggregateService
{
    private readonly ILogger<AggregateService> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ISyntaxService _syntaxService;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IClassFactory _classFactory;
    private readonly IProjectService _projectService;
    private readonly IFileFactory _fileFactory;
    private readonly IFileProvider _fileProvider;
    private readonly IAggregateModelFactory _aggregateModelFactory;
    private readonly ICqrsFactory _cqrsFactory;

    public AggregateService(
        ILogger<AggregateService> logger,
        INamingConventionConverter namingConventionConverter,
        ISyntaxService syntaxService,
        IArtifactGenerator artifactGenerator,
        IClassFactory classFactory,
        IProjectService projectService,
        IFileFactory fileFactory,
        IFileProvider fileProvider,
        IAggregateModelFactory aggregateModelFactory,
        ICqrsFactory cqrsFactory)
    {
        _syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _aggregateModelFactory = aggregateModelFactory ?? throw new ArgumentException(nameof(aggregateModelFactory));
        _cqrsFactory = cqrsFactory ?? throw new ArgumentNullException(nameof(cqrsFactory));
    }

    public async Task<ClassModel> Add(string name, string properties, string directory, string serviceName)
    {
        _logger.LogInformation("Add");

        EnsureCorePackagesAreInstalled(directory);

        EnsureCoreFilesAreAdded(directory);

        if (string.IsNullOrEmpty(serviceName))
        {
            var projectPath = _fileProvider.Get("*.csproj", directory);

            serviceName = Path.GetFileNameWithoutExtension(projectPath).Split('.').First();
        }

        var classModel = _syntaxService.SolutionModel?.GetClass(name, serviceName);

        if (classModel == null)
        {
            classModel = _classFactory.CreateEntity(name, properties);
        }

        var model = await _aggregateModelFactory.CreateAsync(name, classModel.Properties);

        await _artifactGenerator.GenerateAsync(model);

        var fileModel = _fileFactory.CreateDbContextInterface(directory);

        await _artifactGenerator.GenerateAsync(fileModel);

        return classModel;
    }

    private void EnsureCorePackagesAreInstalled(string directory)
    {
        _projectService.PackageAdd("MediatR", directory);
        _projectService.PackageAdd("FluentValidation", directory);
        _projectService.PackageAdd("Microsoft.EntityFrameworkCore", directory);
        _projectService.PackageAdd("Microsoft.Extensions.Logging.Abstractions", directory);
    }

    private void EnsureCoreFilesAreAdded(string directory)
    {
        _projectService.CoreFilesAdd(directory);
    }

    public async Task CommandCreate(string routeType, string name, string aggregate, string properties, string directory)
    {
        var serviceName = Path.GetFileNameWithoutExtension(_fileProvider.Get(CSharpProject, directory)).Split('.').First();

        var classModel = _syntaxService.SolutionModel?.GetClass(aggregate, serviceName);

        if (classModel == null)
        {
            classModel = _classFactory.CreateEntity(aggregate, properties);
        }

        var commandModel = new CommandModel(); //(serviceName, classModel, _namingConventionConverter, name: name, routeType: routeType switch
/*        {
            "create" => RouteType.Create,
            "update" => RouteType.Update,
            "delete" => RouteType.Delete,
            _ => throw new NotSupportedException()
        })*/;

        var model = new CodeFileModel<CommandModel>(commandModel, commandModel.UsingDirectives, commandModel.Name, directory, ".cs");

        await _artifactGenerator.GenerateAsync(model);
    }

    public async Task QueryCreateAsync(string routeType, string name, string aggregate, string properties, string directory)
    {
        var rootNamespace = Path.GetFileNameWithoutExtension(_fileProvider.Get(CSharpProject, directory)).Split('.').First();

        var queryModel = await _cqrsFactory.CreateQueryAsync(routeType, name, properties);

        var model = new CodeFileModel<QueryModel>(queryModel, queryModel.UsingDirectives, queryModel.Name, directory, CSharpFile);

        await _artifactGenerator.GenerateAsync(model);
    }
}


