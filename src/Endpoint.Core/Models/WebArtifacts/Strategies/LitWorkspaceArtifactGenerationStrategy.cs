using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System.IO;

namespace Endpoint.Core.Models.WebArtifacts.Strategies;

public class LitWorkspaceArtifactGenerationStrategy : ArtifactGenerationStrategyBase<LitWorkspaceModel>
{
    private readonly ILogger<LitWorkspaceArtifactGenerationStrategy> _logger;
    private readonly ICommandService _commandService;
    private readonly IFileSystem _fileSystem;
    public LitWorkspaceArtifactGenerationStrategy(
        IServiceProvider serviceProvider,
        ILogger<LitWorkspaceArtifactGenerationStrategy> logger,
        IFileSystem fileSystem,
        ICommandService commandService) 
        : base(serviceProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, LitWorkspaceModel model, dynamic context = null)
    {
        _logger.LogInformation("Generating artifact for {0}.", model);

        _fileSystem.CreateDirectory(model.Directory);

        _commandService.Start("npm init -y", model.Directory);

        _commandService.Start("npm i lit", model.Directory);

        _commandService.Start("npm i -D typescript", model.Directory);

        var packageJsonPath = $"{model.Directory}{Path.DirectorySeparatorChar}package.json";

        var packageJsonObject = JObject.Parse(File.ReadAllText(packageJsonPath));

        packageJsonObject.AddScript("build:watch", "tsc --watch");


        DefaultContractResolver contractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        File.WriteAllText(packageJsonPath,JsonConvert.SerializeObject(packageJsonObject, new JsonSerializerSettings
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