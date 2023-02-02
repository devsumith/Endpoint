// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files.Commands;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Text;
using Microsoft.CSharp;

namespace Endpoint.Core.Models.Artifacts.Files.Strategies;

public class CopyrightAddArtifactGenerationStrategy : ArtifactGenerationStrategyBase<FileReferenceModel>
{
    private readonly ILogger<CopyrightAddArtifactGenerationStrategy> _logger;
    private readonly ITemplateLocator _templateLocator;
    private readonly IFileSystem _fileSystem;

    public CopyrightAddArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<CopyrightAddArtifactGenerationStrategy> logger,
        IFileSystem fileSystem,
        ITemplateLocator templateLocator) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
    }

    public override int Priority => 10;

    public override bool CanHandle(object model, dynamic context)
    {
        return model is FileReferenceModel && context != null && context is CopyrightAdd;
    }
    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, FileReferenceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);
        
        var copyright = string.Join(Environment.NewLine, _templateLocator.Get("Copyright"));

        var ignore = model.Path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}nupkg{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}Properties{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}");

        var extension = Path.GetExtension(model.Path);

        var validExtension = extension == ".cs" || extension == ".ts";

        if (validExtension && !ignore)
        {
            var originalFileContents = _fileSystem.ReadAllText(model.Path);

            if (originalFileContents.Contains(copyright) == false)
            {
                var newFileContentsBuilder = new StringBuilder();

                newFileContentsBuilder.AppendLine(copyright);

                newFileContentsBuilder.AppendLine();

                newFileContentsBuilder.AppendLine(originalFileContents);

                _fileSystem.WriteAllText(model.Path, newFileContentsBuilder.ToString());
            }
        }
    }
}