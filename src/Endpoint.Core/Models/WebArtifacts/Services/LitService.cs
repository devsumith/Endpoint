using Endpoint.Core.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Endpoint.Core.Models.WebArtifacts.Services;

public class LitService: ILitService
{
    private readonly ILogger<LitService> _logger;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;

    public LitService(
        ILogger<LitService> logger,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
    }

    public async Task WorkspaceCreate(string name, string rootDirectory)
    {
        var model = new LitWorkspaceModel(name, rootDirectory);

        _artifactGenerationStrategyFactory.CreateFor(model);
    }

}

