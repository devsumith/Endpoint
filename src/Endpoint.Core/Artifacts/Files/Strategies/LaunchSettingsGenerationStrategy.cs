// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using System.Text;

namespace Endpoint.Core.Artifacts.Files.Strategies;
public class LaunchSettingsFileGenerationStrategy : FileGenerationStrategy, IArtifactGenerationStrategy<LaunchSettingsFileModel>
{
    private readonly ITemplateProcessor _templateProcessor;
    private readonly ITemplateLocator _templateLocator;

    public LaunchSettingsFileGenerationStrategy(IServiceProvider serviceProvider, ITemplateProcessor templateProcessor, IFileSystem fileSystem, ITemplateLocator templateLocator)
        :base(default, fileSystem)
    {
        _templateProcessor = templateProcessor;
        _templateLocator = templateLocator;
    }

    public async Task GenerateAsync(IArtifactGenerator artifactGenerator, LaunchSettingsFileModel model, dynamic context = null)
    {
        var builder = new StringBuilder();

        var template = _templateLocator.Get("LaunchSettings");

        builder.AppendLine(_templateProcessor.Process(template, model));

        model.Content = builder.ToString();

        await base.GenerateAsync(artifactGenerator, model, null);
    }
}

