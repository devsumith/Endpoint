using Endpoint.Core.Models.Options;
using Endpoint.Core.Services;
using Endpoint.Core.ValueObjects;

namespace Endpoint.Core.Models.Artifacts.Files;
public class LaunchSettingsFileGenerationStrategy : ILaunchSettingsFileGenerationStrategy
{
    private readonly ITemplateProcessor _templateProcessor;
    private readonly IFileSystem _fileSystem;
    private readonly ITemplateLocator _templateLocator;

    public LaunchSettingsFileGenerationStrategy(ITemplateProcessor templateProcessor, IFileSystem fileSystem, ITemplateLocator templateLocator)
    {
        _templateProcessor = templateProcessor;
        _fileSystem = fileSystem;
        _templateLocator = templateLocator;
    }

    public void Create(SettingsModel settings)
    {
        var template = _templateLocator.Get("LaunchSettings");

        var tokens = new TokensBuilder()
            .With(nameof(settings.RootNamespace), (Token)settings.RootNamespace)
            .With("Directory", (Token)settings.ApiDirectory)
            .With("Namespace", (Token)settings.ApiNamespace)
            .With(nameof(settings.Port), (Token)$"{settings.Port}")
            .With(nameof(settings.SslPort), (Token)$"{settings.SslPort}")
            .With("ProjectName", (Token)settings.ApiNamespace)
            .With("LaunchUrl", (Token)"")
            .Build();

        var contents = string.Join(Environment.NewLine, _templateProcessor.Process(template, tokens));

        _fileSystem.WriteAllText($@"{settings.ApiDirectory}/Properties/launchSettings.json", contents);
    }
}
