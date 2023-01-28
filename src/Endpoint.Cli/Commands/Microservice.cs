using CommandLine;
using Endpoint.Core;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Git;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Solutions.Crerate;
using Endpoint.Core.Strategies.Solutions.Update;
using Endpoint.Core.Strategies.WorkspaceSettingss.Update;
using MediatR;
using Microsoft.Extensions.Logging;
using Nelibur.ObjectMapper;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static Endpoint.Core.Constants.SolutionTemplates;

namespace Endpoint.Cli.Commands;


[Verb("microservice")]
public class MicroserviceRequest : IRequest<Unit>
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('g', "create-git-repository")]
    public bool CreateGitRepository { get; set; } = false;

    [Option('t', "template")]
    public string TemplateType { get; set; } = CleanArchitectureByJasonTalyor;

    [Option('p', "properties")]
    public string Properties { get; set; } = Environment.GetEnvironmentVariable($"{nameof(Endpoint)}:Properties");

    [Option('r', "resource")]
    public string Resource { get; set; } = Environment.GetEnvironmentVariable($"{nameof(Endpoint)}:Resource");

    [Option("db-context-name")]
    public string DbContextName { get; set; } = Environment.GetEnvironmentVariable($"{nameof(Endpoint)}:DbContextName");

    [Option('w', "workspace-name")]
    public string WorkspaceName { get; set; }

    [Option('d', "directory")]
    public string Directory { get; set; } = Environment.CurrentDirectory;
}

public class MicroserviceRequestHandler : IRequestHandler<MicroserviceRequest, Unit>
{
    private readonly ILogger<MicroserviceRequestHandler> _logger;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
    private readonly ISolutionSettingsFileGenerationStrategyFactory _factory;
    private readonly IWorkspaceGenerationStrategyFactory _workspaceSettingsGenerationStrategyFactory;
    private readonly ISolutionUpdateStrategyFactory _solutionUpdateStrategyFactory;
    private readonly IWorkspaceSettingsUpdateStrategyFactory _workspaceSettingsUpdateStrategyFactory;
    private readonly IFileSystem _fileSystem;
    private readonly ISolutionModelFactory _solutionModelFactory;

    public MicroserviceRequestHandler(ISolutionModelFactory solutionModelFactory, ILogger<MicroserviceRequestHandler> logger, IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ISolutionSettingsFileGenerationStrategyFactory factory, IWorkspaceGenerationStrategyFactory workspaceGenerationStrategyFactory, ISolutionUpdateStrategyFactory solutionUpdateStrategyFactory, IFileSystem fileSystem, IWorkspaceSettingsUpdateStrategyFactory workspaceSettingsUpdateStrategyFactory)
    {
        _solutionModelFactory = solutionModelFactory;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _workspaceSettingsGenerationStrategyFactory = workspaceGenerationStrategyFactory;
        _solutionUpdateStrategyFactory = solutionUpdateStrategyFactory;
        _fileSystem = fileSystem;
        _workspaceSettingsUpdateStrategyFactory = workspaceSettingsUpdateStrategyFactory;
    }

    public SolutionModel CreateMinimalSolutionModel(CreateCleanArchitectureMicroserviceOptions createMicroserviceOptions, MicroserviceRequest request)
    {
        if (IsExistingWorkspace(request.Directory))
        {
            return _solutionModelFactory.Minimal(new CreateEndpointSolutionOptions
            {
                Name = createMicroserviceOptions.Name,
                Directory = createMicroserviceOptions.Directory,
                SolutionDirectory = $"{createMicroserviceOptions.SolutionDirectory}",
                Properties = request.Properties,
                Resource = request.Resource,
                DbContextName = request.DbContextName
            });
        }

        return _solutionModelFactory.Minimal(new CreateEndpointSolutionOptions
        {
            Name = createMicroserviceOptions.Name,
            Directory = $"{createMicroserviceOptions.Directory}",
            SolutionDirectory = $"{createMicroserviceOptions.Directory}{Path.DirectorySeparatorChar}{createMicroserviceOptions.Name}",
            Properties = request.Properties,
            Resource = request.Resource,
            DbContextName = request.DbContextName
        });
    }

    public async Task<Unit> Handle(MicroserviceRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Handled: {nameof(MicroserviceRequest)}");

        ResolveOrCreateWorkspaceOptions resolveOrCreateWorkspaceOptions = IsExistingWorkspace(request.Directory) ? new ResolveOrCreateWorkspaceOptions()
        {
            Directory = Directory.GetParent(request.Directory).FullName,
            Name = Path.GetFileName(request.Directory)
        } : TinyMapper.Map<ResolveOrCreateWorkspaceOptions>(request);

        CreateCleanArchitectureMicroserviceOptions createMicroserviceOptions = IsExistingWorkspace(request.Directory) ? new CreateCleanArchitectureMicroserviceOptions()
        {
            Directory = $"{Directory.GetParent(request.Directory)}",
            SolutionDirectory = request.Directory,
            Name = request.Name
        } : TinyMapper.Map<CreateCleanArchitectureMicroserviceOptions>(request); ;

        (WorkspaceSettingsModel previousWorkspaceSettings, WorkspaceSettingsModel nextWorkspaceSettings) = CreateOrResolvePreviousAndNextWorkspaceSettings(resolveOrCreateWorkspaceOptions);

        SolutionModel solutionModel = request.TemplateType switch
        {
            CleanArchitectureByJasonTalyor => _solutionModelFactory.CleanArchitectureMicroservice(createMicroserviceOptions),
            Minimal => CreateMinimalSolutionModel(createMicroserviceOptions, request),
            _ => throw new NotImplementedException()
        };

        _artifactGenerationStrategyFactory.CreateFor(solutionModel);

        

        _factory.CreateFor(default);


        nextWorkspaceSettings.SolutionSettings.Add(default);


        _workspaceSettingsUpdateStrategyFactory.UpdateFor(previousWorkspaceSettings, nextWorkspaceSettings);

        if (!IsExistingWorkspace(request.Directory) && request.CreateGitRepository)
            _artifactGenerationStrategyFactory.CreateFor(new GitModel(request.WorkspaceName)
            {
                Directory = solutionModel.SolutionDirectory,
            });

        return new();
    }


    public Tuple<WorkspaceSettingsModel, WorkspaceSettingsModel> CreateOrResolvePreviousAndNextWorkspaceSettings(ResolveOrCreateWorkspaceOptions resolveOrCreateWorkspaceOptions)
    {

        throw new NotImplementedException();
    }

    public bool IsExistingWorkspace(string directory) => _fileSystem.Exists($"{directory}{Path.DirectorySeparatorChar}Workspace.json");
}