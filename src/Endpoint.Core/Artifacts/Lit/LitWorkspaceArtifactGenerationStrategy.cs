// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Endpoint.Core.Artifacts.Lit;

public class LitWorkspaceArtifactGenerationStrategy : GenericArtifactGenerationStrategy<LitWorkspaceModel>
{
    private readonly ILogger<LitWorkspaceArtifactGenerationStrategy> _logger;
    private readonly ICommandService _commandService;
    private readonly IFileSystem _fileSystem;
    private readonly IFileFactory _fileFactory;

    public LitWorkspaceArtifactGenerationStrategy(
        ILogger<LitWorkspaceArtifactGenerationStrategy> logger,
        IFileSystem fileSystem,
        ICommandService commandService,
        IFileFactory fileFactory,
        ITemplateLocator templateLocator,
        ITemplateProcessor templateProcessor
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
    }

    public override async Task GenerateAsync(IArtifactGenerator artifactGenerator, LitWorkspaceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        if(_fileSystem.Directory.Exists(model.Directory))
        {
            _fileSystem.Directory.Delete(model.Directory, true);
        }

        _fileSystem.Directory.CreateDirectory(model.Directory);

        var tsConfigModel = new TemplatedFileModel("LitTsConfigJson", "tsconfig", model.Directory, ".json");

        var packageJsonModel = new TemplatedFileModel("LitPackageJson", "package", model.Directory, ".json", new TokensBuilder()
            .With("name", model.Name)
            .Build());

        var webPackConfig = _fileFactory.CreateTemplate("LitWebpackConfig", "webpack.config", model.Directory, ".js");

        var indexTsModel = new FileModel("index", Path.Combine(model.Directory, "src"), ".ts") { Body = $"console.log(\"{model.Name}\")"};

        var indexHtmlModel = new TemplatedFileModel("LitIndexHtml", "index", model.Directory, ".html", new TokensBuilder()
            .With("name", model.Name)
            .Build());

        await artifactGenerator.GenerateAsync(webPackConfig);

        await artifactGenerator.GenerateAsync(packageJsonModel);

        await artifactGenerator.GenerateAsync(tsConfigModel);

        await artifactGenerator.GenerateAsync(indexTsModel);

        await artifactGenerator.GenerateAsync(indexHtmlModel);

        _commandService.Start("npm install --force", model.Directory);

        _commandService.Start("ts-jest config:init", model.Directory);

        _commandService.Start("code .", model.Directory);

    }
}
