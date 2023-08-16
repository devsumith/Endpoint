// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.Text;

namespace Endpoint.Core.Artifacts.Files.Strategies;
public class LaunchSettingsFileGenerationStrategy : FileGenerationStrategy, IGenericArtifactGenerationStrategy<LaunchSettingsFileModel>
{
    private readonly ITemplateProcessor _templateProcessor;

    public LaunchSettingsFileGenerationStrategy(ILoggerFactory loggerFactory, ITemplateProcessor templateProcessor, IFileSystem fileSystem, ITemplateLocator templateLocator)
        :base(loggerFactory.CreateLogger<FileGenerationStrategy>(), fileSystem, templateLocator)
    {
        _templateProcessor = templateProcessor;
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

