using Endpoint.Core.Abstractions;
using Endpoint.Core.Factories;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Models.Git;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Solutions.Crerate;
using Endpoint.Core.Strategies.Solutions.Update;
using Endpoint.Core.Strategies.WorkspaceSettingss.Update;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Endpoint.Core.Strategies.Workspaces
{
    public class WorkspaceCreateOrUpdateStrategyFactory
    {
    }
    public interface IWorkspaceCreateOrUpdateStrategyFactory
    {
    }
    public class WorkspaceCreateOrUpdateStrategy
    {
        private readonly ILogger _logger;
        private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;
        private readonly ISolutionSettingsFileGenerationStrategyFactory _factory;
        private readonly IGitGenerationStrategyFactory _gitGenerationStrategyFactory;
        private readonly IWorkspaceGenerationStrategyFactory _workspaceSettingsGenerationStrategyFactory;
        private readonly ISolutionUpdateStrategyFactory _solutionUpdateStrategyFactory;
        private readonly IWorkspaceSettingsUpdateStrategyFactory _workspaceSettingsUpdateStrategyFactory;
        private readonly IFileSystem _fileSystem;
        public WorkspaceCreateOrUpdateStrategy(ILogger logger, IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory, ISolutionSettingsFileGenerationStrategyFactory factory, IGitGenerationStrategyFactory gitGenerationStrategyFactory, IWorkspaceGenerationStrategyFactory workspaceGenerationStrategyFactory, ISolutionUpdateStrategyFactory solutionUpdateStrategyFactory, IFileSystem fileSystem, IWorkspaceSettingsUpdateStrategyFactory workspaceSettingsUpdateStrategyFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _gitGenerationStrategyFactory = gitGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(gitGenerationStrategyFactory));
            _workspaceSettingsGenerationStrategyFactory = workspaceGenerationStrategyFactory;
            _solutionUpdateStrategyFactory = solutionUpdateStrategyFactory;
            _fileSystem = fileSystem;
            _workspaceSettingsUpdateStrategyFactory = workspaceSettingsUpdateStrategyFactory;
        }

        public Tuple<SolutionModel, SolutionModel> CreateOrResolvePreviousAndNextWorkspaceSolutions(ResolveOrCreateWorkspaceOptions resolveOrCreateWorkspaceOptions)
        {

            if (!_fileSystem.Exists($"{resolveOrCreateWorkspaceOptions.Directory}{Path.DirectorySeparatorChar}{resolveOrCreateWorkspaceOptions.Name}{Path.DirectorySeparatorChar}Workspace.sln"))
            {
                var newWorkspaceSolutionModel = new SolutionModelFactory().Workspace(resolveOrCreateWorkspaceOptions);

                _artifactGenerationStrategyFactory.CreateFor(newWorkspaceSolutionModel);

                return new(new SolutionModelFactory().Workspace(resolveOrCreateWorkspaceOptions), new SolutionModelFactory().Workspace(resolveOrCreateWorkspaceOptions));
            }

            return new(new SolutionModelFactory().Resolve(resolveOrCreateWorkspaceOptions), new SolutionModelFactory().Resolve(resolveOrCreateWorkspaceOptions));
        }

        public Tuple<WorkspaceSettingsModel, WorkspaceSettingsModel> CreateOrResolvePreviousAndNextWorkspaceSettings(ResolveOrCreateWorkspaceOptions resolveOrCreateWorkspaceOptions)
        {

            if (!_fileSystem.Exists($"{resolveOrCreateWorkspaceOptions.Directory}{Path.DirectorySeparatorChar}{resolveOrCreateWorkspaceOptions.Name}{Path.DirectorySeparatorChar}Workspace.json"))
            {
                _workspaceSettingsGenerationStrategyFactory.CreateFor(WorkspaceSettingsModelFactory.Create(resolveOrCreateWorkspaceOptions));

                return new(WorkspaceSettingsModelFactory.Create(resolveOrCreateWorkspaceOptions), WorkspaceSettingsModelFactory.Create(resolveOrCreateWorkspaceOptions));
            }

            return new(WorkspaceSettingsModelFactory.Resolve(resolveOrCreateWorkspaceOptions), WorkspaceSettingsModelFactory.Resolve(resolveOrCreateWorkspaceOptions));
        }
    }
    public interface IWorkspaceCreateOrUpdateStrategy
    {
    }

}
