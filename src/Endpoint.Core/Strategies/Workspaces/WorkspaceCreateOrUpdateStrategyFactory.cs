using Endpoint.Core.Factories;
using Endpoint.Core.Models.Artifacts;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Common.Git;
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
        private readonly ISolutionGenerationStrategy _solutionGenerationStrategy;
        private readonly ISolutionSettingsFileGenerationStrategyFactory _factory;
        private readonly IGitGenerationStrategyFactory _gitGenerationStrategyFactory;
        private readonly IWorkspaceGenerationStrategyFactory _workspaceSettingsGenerationStrategyFactory;
        private readonly ISolutionUpdateStrategyFactory _solutionUpdateStrategyFactory;
        private readonly IWorkspaceSettingsUpdateStrategyFactory _workspaceSettingsUpdateStrategyFactory;
        private readonly IFileSystem _fileSystem;
        public WorkspaceCreateOrUpdateStrategy(ILogger logger, ISolutionGenerationStrategy solutionGenerationStrategy, ISolutionSettingsFileGenerationStrategyFactory factory, IGitGenerationStrategyFactory gitGenerationStrategyFactory, IWorkspaceGenerationStrategyFactory workspaceGenerationStrategyFactory, ISolutionUpdateStrategyFactory solutionUpdateStrategyFactory, IFileSystem fileSystem, IWorkspaceSettingsUpdateStrategyFactory workspaceSettingsUpdateStrategyFactory)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _solutionGenerationStrategy = solutionGenerationStrategy ?? throw new ArgumentNullException(nameof(solutionGenerationStrategy));
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
                var newWorkspaceSolutionModel = SolutionModelFactory.Workspace(resolveOrCreateWorkspaceOptions);

                _solutionGenerationStrategy.Create(newWorkspaceSolutionModel);

                return new(SolutionModelFactory.Workspace(resolveOrCreateWorkspaceOptions), SolutionModelFactory.Workspace(resolveOrCreateWorkspaceOptions));
            }

            return new(SolutionModelFactory.Resolve(resolveOrCreateWorkspaceOptions), SolutionModelFactory.Resolve(resolveOrCreateWorkspaceOptions));
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
