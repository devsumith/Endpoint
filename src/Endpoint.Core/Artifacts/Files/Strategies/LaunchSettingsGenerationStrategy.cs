// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using System.Text;

namespace Endpoint.Core.Artifacts.Files.Strategies;
public class LaunchSettingsFileGenerationStrategy : IArtifactGenerationStrategy<LaunchSettingsFileModel>
{
    private readonly ITemplateProcessor _templateProcessor;
    private readonly IFileSystem _fileSystem;
    private readonly ITemplateLocator _templateLocator;

    public LaunchSettingsFileGenerationStrategy(IServiceProvider serviceProvider, ITemplateProcessor templateProcessor, IFileSystem fileSystem, ITemplateLocator templateLocator)

    {
        _templateProcessor = templateProcessor;
        _fileSystem = fileSystem;
        _templateLocator = templateLocator;
    }

    public int Priority => 0;

    public async Task GenerateAsync(IArtifactGenerator artifactGenerator, LaunchSettingsFileModel model, dynamic context = null)
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

