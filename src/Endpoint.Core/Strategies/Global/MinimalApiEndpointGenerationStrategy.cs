using Endpoint.Core.Factories;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Core.Strategies.Global
{
    internal class MinimalApiEndpointGenerationStrategy : IEndpointGenerationStrategy
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

            _commandService.Start($"code .", workspaceDirectory);

            _commandService.Start($"endpoint git {options.Name}", workspaceDirectory);

            var solutionModel = SolutionModelFactory.Minimal(new()
            {
                Name = options.Name,
                Port = options.Port,
                Properties = options.Properties,
                Resource = options.Resource,
                Monolith = options.Monolith,
                Minimal = options.Minimal,
                DbContextName = options.DbContextName,
                ShortIdPropertyName = options.ShortIdPropertyName,
                NumericIdPropertyDataType = options.NumericIdPropertyDataType,
                Directory = options.Directory
            });

            _solutionGenerationStrategy.Create(solutionModel);
        }
    }
}
