// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Endpoint.Core.Syntax.Entities.Aggregate;

public class AggregateService : IAggregateService
{
    private readonly ILogger<AggregateService> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ISyntaxService _syntaxService;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IClassModelFactory _classModelFactory;
    private readonly IProjectService _projectService;
    private readonly IFileFactory _fileModelFactory;
    private readonly IFileProvider _fileProvider;

    public AggregateService(
        ILogger<AggregateService> logger,
        INamingConventionConverter namingConventionConverter,
        ISyntaxService syntaxService,
        IArtifactGenerator artifactGenerator,
        IClassModelFactory classModelFactory,
        IProjectService projectService,
        IFileFactory fileModelFactory,
        IFileProvider fileProvider)
    {
        _syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
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
            classModel = _classModelFactory.CreateEntity(name, properties);
        }

        var model = new AggregatesModel(_namingConventionConverter, serviceName, classModel, directory);

        await _artifactGenerator.CreateAsync(model);

        var fileModel = _fileModelFactory.CreateDbContextInterface(directory);

        await _artifactGenerator.CreateAsync(fileModel);

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
        var serviceName = Path.GetFileNameWithoutExtension(_fileProvider.Get("*.csproj", directory)).Split('.').First();

        var classModel = _syntaxService.SolutionModel?.GetClass(aggregate, serviceName);

        if (classModel == null)
        {
            classModel = _classModelFactory.CreateEntity(aggregate, properties);
        }

        var commandModel = new CommandModel(serviceName, classModel, _namingConventionConverter, name: name, routeType: routeType switch
        {
            "create" => RouteType.Create,
            "update" => RouteType.Update,
            "delete" => RouteType.Delete,
            _ => throw new NotSupportedException()
        });

        var model = new ObjectFileModel<CommandModel>(commandModel, commandModel.UsingDirectives, commandModel.Name, directory, ".cs");

        await _artifactGenerator.CreateAsync(model);
    }

    public async Task QueryCreate(string routeType, string name, string aggregate, string properties, string directory)
    {
        var serviceName = Path.GetFileNameWithoutExtension(_fileProvider.Get("*.csproj", directory)).Split('.').First();

        var classModel = _syntaxService.SolutionModel?.GetClass(aggregate, serviceName);

        if (classModel == null)
        {
            classModel = _classModelFactory.CreateEntity(aggregate, properties);
        }

        var queryModel = new QueryModel(serviceName, _namingConventionConverter, classModel, name: name, routeType: routeType.ToLower() switch
        {
            "get" => RouteType.Get,
            "getbyid" => RouteType.GetById,
            "page" => RouteType.Page,
            _ => throw new NotSupportedException()
        });

        var model = new ObjectFileModel<QueryModel>(queryModel, queryModel.UsingDirectives, queryModel.Name, directory, ".cs");

        await _artifactGenerator.CreateAsync(model);
    }
}


