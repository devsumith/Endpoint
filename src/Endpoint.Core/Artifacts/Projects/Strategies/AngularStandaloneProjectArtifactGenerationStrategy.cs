using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Projects.Strategies;

public class AngularStandaloneProjectArtifactGenerationStrategy : IGenericArtifactGenerationStrategy<ProjectModel>
{
    private readonly ILogger<AngularStandaloneProjectArtifactGenerationStrategy> _logger;
    private readonly IFileSystem _fileSystem;
    private readonly ITemplateLocator _templateLocator;
    private readonly ITemplateProcessor _templateProcessor;
    public AngularStandaloneProjectArtifactGenerationStrategy(
        ILogger<AngularStandaloneProjectArtifactGenerationStrategy> logger,
        IFileSystem fileSystem,
        ITemplateLocator templateLocator,
        ITemplateProcessor templateProcessor)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        _templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
    }

    public async Task<bool> GenerateAsync(IArtifactGenerator generator, object target, dynamic context = null)
    {
        if (target is ProjectModel model && model.Extension == ".esproj")
        {
            await GenerateAsync(generator, model, context);

            return true;
        }
        else
            return false;
    }

    public virtual int GetPriority() => 1;

    public async Task GenerateAsync(IArtifactGenerator generator, ProjectModel model, dynamic? context = null)
    {
        var template = string.Join(Environment.NewLine, _templateLocator.Get("EsProj"));

        var result = _templateProcessor.Process(template, new TokensBuilder()
            .With("projectName", model.Name)
            .Build());

        _fileSystem.Directory.CreateDirectory(model.Directory);

        _fileSystem.File.WriteAllText(model.Path, result);
    }
}