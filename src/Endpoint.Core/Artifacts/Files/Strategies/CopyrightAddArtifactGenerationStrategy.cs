// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using System.Text;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class CopyrightAddArtifactGenerationStrategy : GenericArtifactGenerationStrategy<FileReferenceModel>
{
    private readonly ILogger<CopyrightAddArtifactGenerationStrategy> logger;
    private readonly ITemplateLocator templateLocator;
    private readonly IFileSystem fileSystem;

    public CopyrightAddArtifactGenerationStrategy(

        ILogger<CopyrightAddArtifactGenerationStrategy> logger,
        IFileSystem fileSystem,
        ITemplateLocator templateLocator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
    }

    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, FileReferenceModel model)
    {
        logger.LogInformation("Generating artifact for {0}.", model);

        var copyright = string.Join(Environment.NewLine, templateLocator.Get("Copyright"));

        var ignore = model.Path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}nupkg{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}Properties{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}");

        var extension = Path.GetExtension(model.Path);

        var validExtension = extension == ".cs" || extension == ".ts" || extension == ".js";

        if (validExtension && !ignore)
        {
            var originalFileContents = fileSystem.File.ReadAllText(model.Path);

            if (originalFileContents.Contains(copyright) == false)
            {
                var newFileContentsBuilder = new StringBuilder();

                newFileContentsBuilder.AppendLine(copyright);

                newFileContentsBuilder.AppendLine();

                newFileContentsBuilder.AppendLine(originalFileContents);

                fileSystem.File.WriteAllText(model.Path, newFileContentsBuilder.ToString());
            }
        }
    }
}