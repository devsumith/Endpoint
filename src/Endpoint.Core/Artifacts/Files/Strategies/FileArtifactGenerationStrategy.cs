// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class FileGenerationStrategy : GenericArtifactGenerationStrategy<FileModel>
{
    private readonly ILogger<FileGenerationStrategy> _logger;
    protected readonly IFileSystem _fileSystem;
    protected readonly ITemplateLocator _templateLocator;

    public FileGenerationStrategy(
        ILogger<FileGenerationStrategy> logger,
        IFileSystem fileSystem,
        ITemplateLocator templateLocator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
    }

    public override async Task GenerateAsync(IArtifactGenerator generator, FileModel model, dynamic? context = null)
    {
        _logger.LogInformation("Generating file artifact. {name}", model.Name);

        var copyright = string.Join(Environment.NewLine, _templateLocator.Get("Copyright"));


        var parts = Path.GetDirectoryName(model.Path).Split(Path.DirectorySeparatorChar);

        for (var i = 1; i <= parts.Length; i++)
        {
            var dir = string.Join(Path.DirectorySeparatorChar, parts.Take(i));

            if (!Directory.Exists(dir))
                _fileSystem.Directory.CreateDirectory(dir);
        }

        var ignore = model.Path.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}nupkg{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}Properties{Path.DirectorySeparatorChar}")
            || model.Path.Contains($"{Path.DirectorySeparatorChar}node_modules{Path.DirectorySeparatorChar}");

        var extension = Path.GetExtension(model.Path);

        var validExtension = extension == ".cs" || extension == ".ts" || extension == ".js";

        _fileSystem.File.WriteAllText(model.Path, validExtension && !ignore ? string.Join(Environment.NewLine, new string[]
        {
            copyright,
            model.Content
        }) : model.Content);

    }
}
