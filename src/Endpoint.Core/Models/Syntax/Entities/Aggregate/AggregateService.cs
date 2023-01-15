using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public class AggregateService: IAggregateService
{
    private readonly ILogger<AggregateService> _logger;
    private readonly INamingConventionConverter _namingConventionConverter;
    private readonly IFileProvider _fileProvider;
    private readonly ISyntaxService _syntaxService;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly IFileSystem _fileSystem;
    public AggregateService(
        ILogger<AggregateService> logger,
        IFileSystem fileSystem,
        INamingConventionConverter namingConventionConverter,
        IFileProvider fileProvider,
        ISyntaxService syntaxService,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory) {

        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _syntaxService = syntaxService ?? throw new ArgumentNullException(nameof(syntaxService));
        _namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
    }

    public async Task Add(string name, string properties, string directory)
    {
        var aggregateDirectory = $"{directory}{Path.DirectorySeparatorChar}{name}Aggregate";

        _fileSystem.CreateDirectory(aggregateDirectory);

        var classModel = _syntaxService.SolutionModel.GetClass(name);

        var dtoModel = classModel.CreateDto();


        AggregateModel model = new AggregateModel(_namingConventionConverter, "", classModel);

        if(classModel != null)
        {
            var classFileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, aggregateDirectory, "cs");

            var dtoFileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, aggregateDirectory, "cs");

            _artifactGenerationStrategyFactory.CreateFor(classFileModel);
        }        
    }

}

