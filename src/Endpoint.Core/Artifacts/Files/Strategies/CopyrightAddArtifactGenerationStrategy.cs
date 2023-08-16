// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class CopyrightAddArtifactGenerationStrategy : GenericArtifactGenerationStrategy<FileReferenceModel>
{
    private readonly ILogger<CopyrightAddArtifactGenerationStrategy> _logger;
    private readonly ITemplateLocator _templateLocator;
    private readonly IFileSystem _fileSystem;

    public CopyrightAddArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<CopyrightAddArtifactGenerationStrategy> logger,
        IFileSystem fileSystem,
        ITemplateLocator templateLocator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
    }

    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, FileReferenceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        var copyright = string.Join(Environment.NewLine, _templateLocator.Get("Copyright"));

        var ignore = model.Path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}nupkg{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}Properties{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}");

        var extension = Path.GetExtension(model.Path);

        var validExtension = extension == ".cs" || extension == ".ts" || extension == ".js";

        if (validExtension && !ignore)
        {
            var originalFileContents = _fileSystem.File.ReadAllText(model.Path);

            if (originalFileContents.Contains(copyright) == false)
            {
                var newFileContentsBuilder = new StringBuilder();

                newFileContentsBuilder.AppendLine(copyright);

                newFileContentsBuilder.AppendLine();

                newFileContentsBuilder.AppendLine(originalFileContents);

                _fileSystem.File.WriteAllText(model.Path, newFileContentsBuilder.ToString());
            }
        }
    }
}