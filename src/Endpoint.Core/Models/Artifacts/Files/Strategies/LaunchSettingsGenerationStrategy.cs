using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using System.Text;

namespace Endpoint.Core.Models.Artifacts.Files.Strategies;
public class LaunchSettingsFileGenerationStrategy : ArtifactGenerationStrategyBase<LaunchSettingsFileModel>
{
    private readonly ITemplateProcessor _templateProcessor;
    private readonly IFileSystem _fileSystem;
    private readonly ITemplateLocator _templateLocator;

    public LaunchSettingsFileGenerationStrategy(IServiceProvider serviceProvider, ITemplateProcessor templateProcessor, IFileSystem fileSystem, ITemplateLocator templateLocator)
        : base(serviceProvider)
    {
        _templateProcessor = templateProcessor;
        _fileSystem = fileSystem;
        _templateLocator = templateLocator;
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, LaunchSettingsFileModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        var template = _templateLocator.Get("LaunchSettings");

        /*        var tokens = new TokensBuilder()
                    .With(nameof(settings.RootNamespace), settings.RootNamespace)
                    .With("Directory", settings.ApiDirectory)
                    .With("Namespace", settings.ApiNamespace)
                    .With(nameof(settings.Port), $"{settings.Port}")
                    .With(nameof(settings.SslPort), $"{settings.SslPort}")
                    .With("ProjectName", settings.ApiNamespace)
                    .With("LaunchUrl", "")
                    .Build();*/

        builder.AppendLine(_templateProcessor.Process(template, model));


        _fileSystem.WriteAllText(model.Path, builder.ToString());
    }
}
