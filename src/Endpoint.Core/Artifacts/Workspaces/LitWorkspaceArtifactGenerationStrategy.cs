// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Lit;
using Endpoint.Core.Extensions;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Artifacts.Workspaces;

public class LitWorkspaceArtifactGenerationStrategy : IArtifactGenerationStrategy<LitWorkspaceModel>
{
    private readonly ILogger<LitWorkspaceArtifactGenerationStrategy> _logger;
    private readonly ICommandService _commandService;
    private readonly IFileSystem _fileSystem;
    private readonly IFileFactory _fileFactory;
    public LitWorkspaceArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<LitWorkspaceArtifactGenerationStrategy> logger,
        IFileSystem fileSystem,
        ICommandService commandService,
        IFileFactory fileFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileFactory = fileFactory ?? throw new ArgumentNullException(nameof(fileFactory));
    }

    public int Priority { get; } = 0;

    public async Task GenerateAsync(IArtifactGenerator artifactGenerator, LitWorkspaceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        _fileSystem.CreateDirectory(model.Directory);

        _fileSystem.CreateDirectory($"{model.Directory}{Path.DirectorySeparatorChar}apps");

        _fileSystem.CreateDirectory($"{model.Directory}{Path.DirectorySeparatorChar}libs");

        _commandService.Start("npm init -y", model.Directory);

        _commandService.Start("npm i lit", model.Directory);

        _commandService.Start("npm i -D typescript jest ts-jest @types/jest", model.Directory);

        foreach (var npmPackage in new List<string>()
        {
            "rxjs",
            "webpack-cli",
            "webpack",
            "ts-loader",
            "raw-loader",
            "html-loader"
        })
        {
            _commandService.Start($"npm install -D {npmPackage} --force", model.Directory);
        }

        _commandService.Start("ts-jest config:init", model.Directory);

        var webPackConfig = _fileFactory.CreateTemplate("Webpack.Config", "webpack.config", model.Directory, "js");

        await artifactGenerator.GenerateAsync(webPackConfig);

        var packageJsonPath = $"{model.Directory}{Path.DirectorySeparatorChar}package.json";

        var packageJsonObject = JObject.Parse(File.ReadAllText(packageJsonPath));

        packageJsonObject.RemoveAllScripts();

        packageJsonObject.AddScripts(new Dictionary<string, string>()
        {
            { "build","tsc" },
            { "dev","webpack --watch" },
            { "test","jest" },
        });

        _fileSystem.WriteAllText($"{model.Directory}lit.json", "{ }");

        DefaultContractResolver contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        _fileSystem.WriteAllText(packageJsonPath, JsonConvert.SerializeObject(packageJsonObject, new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.Indented
        }));

        _fileSystem.WriteAllText($"{model.Directory}{Path.DirectorySeparatorChar}tsconfig.json", JsonConvert.SerializeObject(new TsConfigModel(), new JsonSerializerSettings
        {
            ContractResolver = contractResolver,
            Formatting = Formatting.Indented
        }));
    }
}
