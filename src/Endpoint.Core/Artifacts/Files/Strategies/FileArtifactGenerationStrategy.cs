// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;
using System.Linq;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class FileGenerationStrategy : IArtifactGenerationStrategy<FileModel>
{
    private readonly ILogger<FileGenerationStrategy> _logger;
    protected readonly IFileSystem _fileSystem;

    public FileGenerationStrategy(ILogger<FileGenerationStrategy> logger, IFileSystem fileSystem)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
    }

    public int Priority => 0;

    public async Task GenerateAsync(IArtifactGenerator generator, FileModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating file artifact. {name}", model.Name);

        var parts = Path.GetDirectoryName(model.Path).Split(Path.DirectorySeparatorChar);

        for (var i = 1; i <= parts.Length; i++)
        {
            var dir = string.Join(Path.DirectorySeparatorChar, parts.Take(i));

            if (!Directory.Exists(dir))
                _fileSystem.CreateDirectory(dir);
        }

        _fileSystem.WriteAllText(model.Path, model.Content);

    }
}
