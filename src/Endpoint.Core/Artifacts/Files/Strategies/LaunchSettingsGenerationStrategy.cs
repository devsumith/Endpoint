// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Text;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Files.Strategies;
public class LaunchSettingsFileGenerationStrategy : FileGenerationStrategy, IGenericArtifactGenerationStrategy<LaunchSettingsFileModel>
{
    private readonly ITemplateProcessor templateProcessor;

    public LaunchSettingsFileGenerationStrategy(ILoggerFactory loggerFactory, ITemplateProcessor templateProcessor, IFileSystem fileSystem, ITemplateLocator templateLocator)
        : base(loggerFactory.CreateLogger<FileGenerationStrategy>(), fileSystem, templateLocator)
    {
        this.templateProcessor = templateProcessor;
    }

    public async Task GenerateAsync(IArtifactGenerator artifactGenerator, LaunchSettingsFileModel model)
    {
        var builder = new StringBuilder();

        var template = templateLocator.Get("LaunchSettings");

        builder.AppendLine(templateProcessor.Process(template, model));

        model.Body = builder.ToString();

        await base.GenerateAsync(artifactGenerator, model);
    }
}
