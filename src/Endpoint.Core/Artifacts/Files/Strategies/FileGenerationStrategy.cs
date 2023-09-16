// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.IO;
using System.Linq;
using System.Text;
using Endpoint.Core.Extensions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using static Endpoint.Core.Constants;

namespace Endpoint.Core.Artifacts.Files.Strategies;

public class FileGenerationStrategy : GenericArtifactGenerationStrategy<FileModel>
{
    protected readonly IFileSystem fileSystem;
    private readonly ILogger<FileGenerationStrategy> logger;
    protected readonly ITemplateLocator templateLocator;

    public FileGenerationStrategy(
        ILogger<FileGenerationStrategy> logger,
        IFileSystem fileSystem,
        ITemplateLocator templateLocator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        this.templateLocator = templateLocator ?? throw new ArgumentNullException(nameof(templateLocator));
    }

    public override async Task GenerateAsync(IArtifactGenerator generator, FileModel model)
    {
        logger.LogInformation("Generating file artifact. {name}", model.Name);

        var copyright = templateLocator.GetCopyright();

        var parts = Path.GetDirectoryName(model.Path).Split(Path.DirectorySeparatorChar);

        for (var i = 1; i <= parts.Length; i++)
        {
            var dir = string.Join(Path.DirectorySeparatorChar, parts.Take(i));

            if (!Directory.Exists(dir))
            {
                fileSystem.Directory.CreateDirectory(dir);
            }
        }

        var extension = Path.GetExtension(model.Path);

        var validExtension = extension == ".cs" || extension == ".ts" || extension == ".js";

        fileSystem.File.WriteAllText(model.Path, validExtension && !ExcludePatterns.Any(model.Path.Contains)
            ? new StringBuilder().AppendJoin(Environment.NewLine, copyright, string.Empty, model.Body).ToString()
            : model.Body);
    }
}
