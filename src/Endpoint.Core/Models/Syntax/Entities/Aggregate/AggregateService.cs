using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Projects;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public class AggregateService: IAggregateService
{
    private readonly ILogger<AggregateService> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly ISyntaxService _syntaxService;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;    
    private readonly IClassModelFactory _classModelFactory;
    private readonly IProjectService _projectService;
    private readonly IFileModelFactory _fileModelFactory;
    public AggregateService(
        ILogger<AggregateService> logger,
        INamingConventionConverter namingConventionConverter,
        ISyntaxService syntaxService,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        IClassModelFactory classModelFactory,
        IProjectService projectService,
        IFileModelFactory fileModelFactory) {
        _syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));
        _projectService = projectService ?? throw new ArgumentNullException(nameof(projectService));
        _fileModelFactory = fileModelFactory ?? throw new ArgumentNullException(nameof(fileModelFactory));
    }

    public async Task Add(string name, string properties, string directory, string serviceName)
    {
        _logger.LogInformation("Add");

        EnsureCorePackagesAreInstalled(directory);

        EnsureCoreFilesAreAdded(directory);

        var classModel = _syntaxService.SolutionModel?.GetClass(name, serviceName);

        if(classModel == null)
        {
            classModel = _classModelFactory.CreateEntity(name, properties);
        }

        var model = new AggregateModel(_namingConventionConverter, serviceName, classModel,directory);

        _artifactGenerationStrategyFactory.CreateFor(model);

        var fileModel = _fileModelFactory.CreateDbContextInterface(directory);

        _artifactGenerationStrategyFactory.CreateFor(fileModel);
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
}

