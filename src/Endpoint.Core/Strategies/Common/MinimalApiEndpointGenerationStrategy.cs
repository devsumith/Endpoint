using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Models.Git;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Microsoft.Extensions.Options;
using Nelibur.ObjectMapper;
using System.IO;

namespace Endpoint.Core.Strategies.Common;

public class MinimalApiEndpointGenerationStrategy : ArtifactGenerationStrategyBase<MinimalApiSolutionModel>
{
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly ICommandService _commandService;
    private readonly IFileSystem _fileSystem;
    private readonly ISolutionModelFactory _solutionModelFactory;

    public MinimalApiEndpointGenerationStrategy(IServiceProvider serviceProvider, IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ICommandService commandService, IFileSystem fileSystem, ISolutionModelFactory solutionModelFactory)
        :base(serviceProvider)
    {
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
        _commandService = commandService;
        _fileSystem = fileSystem;
        _solutionModelFactory = solutionModelFactory;
    }

    public override void Create(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, MinimalApiSolutionModel model, dynamic configuration = null)
    {
        var workspaceDirectory = $"{model.Directory}{Path.DirectorySeparatorChar}{model.Name}";

        _fileSystem.CreateDirectory(workspaceDirectory);

        _artifactGenerationStrategyFactory.CreateFor(new GitModel(model.Name)
        {
            Directory = workspaceDirectory,
        });

/*        var solutionOptions = TinyMapper.Map<CreateEndpointSolutionOptions>(options);

        var solutionModel = _solutionModelFactory.Minimal(solutionOptions);

        _artifactGenerationStrategyFactory.CreateFor(solutionModel);

        _commandService.Start(options.VsCode ? "code ." : $"start {options.Name}.sln", workspaceDirectory);*/
    }
}
