// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.Artifacts.Projects.Services;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Classes.Factories;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public class AggregateService : IAggregateService
{
    private readonly ILogger<AggregateService> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ISyntaxService _syntaxService;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly IClassModelFactory _classModelFactory;
    private readonly IProjectService _projectService;
    private readonly IFileModelFactory _fileModelFactory;
    private readonly IFileProvider _fileProvider;

    public AggregateService(
        ILogger<AggregateService> logger,
        INamingConventionConverter namingConventionConverter,
        ISyntaxService syntaxService,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        IClassModelFactory classModelFactory,
        IProjectService projectService,
        IFileModelFactory fileModelFactory,
        IFileProvider fileProvider)
    {
        _syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
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

        _artifactGenerationStrategyFactory.CreateFor(model);

        var fileModel = _fileModelFactory.CreateDbContextInterface(directory);

        _artifactGenerationStrategyFactory.CreateFor(fileModel);

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

    public void CommandCreate(string routeType, string name, string aggregate, string properties, string directory)
    {
        var serviceName = Path.GetFileNameWithoutExtension(_fileProvider.Get("*.csproj", directory)).Split('.').First();

        var classModel = _syntaxService.SolutionModel?.GetClass(aggregate, serviceName);

        if(classModel == null)
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

        var model = new ObjectFileModel<CommandModel>(commandModel, commandModel.UsingDirectives, commandModel.Name, directory, "cs");

        _artifactGenerationStrategyFactory.CreateFor(model);
    }

    public void QueryCreate(string routeType, string name, string aggregate, string properties, string directory)
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

        var model = new ObjectFileModel<QueryModel>(queryModel, queryModel.UsingDirectives, queryModel.Name, directory, "cs");

        _artifactGenerationStrategyFactory.CreateFor(model);
    }
}


