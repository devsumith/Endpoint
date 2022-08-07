using Endpoint.Core.Factories;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Solutions.Crerate;
using Nelibur.ObjectMapper;
using System.IO;

namespace Endpoint.Core.Strategies.Common
{
    public class MinimalApiEndpointGenerationStrategy : IEndpointGenerationStrategy
    {
        private readonly ISolutionGenerationStrategy _solutionGenerationStrategy;
        private readonly ICommandService _commandService;
        private readonly IFileSystem _fileSystem;

        public MinimalApiEndpointGenerationStrategy(ISolutionGenerationStrategy solutionGenerationStrategy, ICommandService commandService, IFileSystem fileSystem)
        {
            _solutionGenerationStrategy = solutionGenerationStrategy;
            _commandService = commandService;
            _fileSystem = fileSystem;
        }
        public int Order => 0;
        public bool CanHandle(CreateEndpointOptions options) => options.Minimal.Value;

        public void Create(CreateEndpointOptions options)
        {
            var workspaceDirectory = $"{options.Directory}{Path.DirectorySeparatorChar}{options.Name}";

            _fileSystem.CreateDirectory(workspaceDirectory);

            _commandService.Start($"endpoint git {options.Name}", workspaceDirectory);

            var solutionOptions = TinyMapper.Map<CreateEndpointSolutionOptions>(options);

            var solutionModel = SolutionModelFactory.Minimal(solutionOptions);

            _solutionGenerationStrategy.Create(solutionModel);

            _commandService.Start(options.VsCode ? "code ." : $"start {options.Name}.sln", workspaceDirectory);

        }
    }
}
