using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Projects.Strategies;

public class AngularStandaloneProjectArtifactGenerationStrategy : IGenericArtifactGenerationStrategy<ProjectModel>
{
    private readonly ILogger<AngularStandaloneProjectArtifactGenerationStrategy> logger;
    private readonly IFileSystem fileSystem;
    private readonly ITemplateLocator templateLocator;
    private readonly ITemplateProcessor templateProcessor;

    public AngularStandaloneProjectArtifactGenerationStrategy(
        ILogger<AngularStandaloneProjectArtifactGenerationStrategy> logger,
        IFileSystem fileSystem,
        ITemplateLocator templateLocator,
        ITemplateProcessor templateProcessor)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
        this.templateProcessor = templateProcessor ?? throw new ArgumentNullException(nameof(templateProcessor));
    }

    public async Task<bool> GenerateAsync(IArtifactGenerator generator, object target)
    {
        if (target is ProjectModel model && model.Extension == ".esproj")
        {
            await GenerateAsync(generator, model);

            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual int GetPriority() => 1;

    public async Task GenerateAsync(IArtifactGenerator generator, ProjectModel model)
    {
        var template = string.Join(Environment.NewLine, templateLocator.Get("EsProj"));

        var result = templateProcessor.Process(template, new TokensBuilder()
            .With("projectName", model.Name)
            .Build());

        fileSystem.Directory.CreateDirectory(model.Directory);

        fileSystem.File.WriteAllText(model.Path, result);
    }
}