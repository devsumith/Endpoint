/*// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Git;
using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
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
public class MicroserviceRequest : IRequest
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

public class MicroserviceRequestHandler : IRequestHandler<MicroserviceRequest>
{
    private readonly ILogger<MicroserviceRequestHandler> _logger;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly ISolutionSettingsFileGenerator _factory;
    private readonly IWorkspaceGenerator _workspaceSettingsGenerator;
    private readonly ISolutionUpdateStrategyFactory _solutionUpdateStrategyFactory;
    private readonly IWorkspaceSettingsUpdateStrategyFactory _workspaceSettingsUpdateStrategyFactory;
    private readonly IFileSystem _fileSystem;
    private readonly ISolutionFactory _solutionFactory;

    public MicroserviceRequestHandler(ISolutionFactory solutionFactory, ILogger<MicroserviceRequestHandler> logger, IArtifactGenerator artifactGenerator, ISolutionSettingsFileGenerator factory, IWorkspaceGenerator workspaceGenerator, ISolutionUpdateStrategyFactory solutionUpdateStrategyFactory, IFileSystem fileSystem, IWorkspaceSettingsUpdateStrategyFactory workspaceSettingsUpdateStrategyFactory)
    {
        _solutionFactory = solutionFactory;
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        _workspaceSettingsGenerator = workspaceGenerator;
        _solutionUpdateStrategyFactory = solutionUpdateStrategyFactory;
        _fileSystem = fileSystem;
        _workspaceSettingsUpdateStrategyFactory = workspaceSettingsUpdateStrategyFactory;
    }

    public async Task<SolutionModel> CreateMinimalSolutionModel(CreateCleanArchitectureMicroserviceOptions createMicroserviceOptions, MicroserviceRequest request)
    {
        if (IsExistingWorkspace(request.Directory))
        {
            return await _solutionFactory.Minimal(new CreateEndpointSolutionOptions
            {
                Name = createMicroserviceOptions.Name,
                Directory = createMicroserviceOptions.Directory,
                SolutionDirectory = $"{createMicroserviceOptions.SolutionDirectory}",
                Properties = request.Properties,
                Resource = request.Resource,
                DbContextName = request.DbContextName
            });
        }

        return await _solutionFactory.Minimal(new CreateEndpointSolutionOptions
        {
            Name = createMicroserviceOptions.Name,
            Directory = $"{createMicroserviceOptions.Directory}",
            SolutionDirectory = $"{createMicroserviceOptions.Directory}{Path.DirectorySeparatorChar}{createMicroserviceOptions.Name}",
            Properties = request.Properties,
            Resource = request.Resource,
            DbContextName = request.DbContextName
        });
    }

    public async Task Handle(MicroserviceRequest request, CancellationToken cancellationToken)
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
            CleanArchitectureByJasonTalyor => await _solutionFactory.CleanArchitectureMicroservice(createMicroserviceOptions),
            Minimal => await CreateMinimalSolutionModel(createMicroserviceOptions, request),
            _ => throw new NotImplementedException()
        };

        await _artifactGenerator.GenerateAsync(solutionModel);



        _factory.CreateFor(default);


        nextWorkspaceSettings.SolutionSettings.Add(default);


        _workspaceSettingsUpdateStrategyFactory.UpdateFor(previousWorkspaceSettings, nextWorkspaceSettings);

        if (!IsExistingWorkspace(request.Directory) && request.CreateGitRepository)
            await _artifactGenerator.GenerateAsync(new GitModel(request.WorkspaceName)
            {
                Directory = solutionModel.SolutionDirectory,
            });


    }


    public Tuple<WorkspaceSettingsModel, WorkspaceSettingsModel> CreateOrResolvePreviousAndNextWorkspaceSettings(ResolveOrCreateWorkspaceOptions resolveOrCreateWorkspaceOptions)
    {

        throw new NotImplementedException();
    }

    public bool IsExistingWorkspace(string directory) => _fileSystem.Exists($"{directory}{Path.DirectorySeparatorChar}Workspace.json");
}
*/