using CommandLine;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;
using Endpoint.Core.Strategies;
using Endpoint.Core.Factories;
using Nelibur.ObjectMapper;
using Endpoint.Core.Options;
using Endpoint.Core;
using Endpoint.Core.Strategies.Common.Git;
using Endpoint.Core.Models;

namespace Endpoint.Application.Commands
{
    public class Microservice
    {
        [Verb("microservice")]
        public class Request : IRequest<Unit> {
            [Option('n',"name")]
            public string Name { get; set; }

            [Option('d', "directory")]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        public class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ILogger _logger;
            private readonly ISolutionGenerationStrategy _solutionGenerationStrategy;
            private readonly ISettingsFileGenerationStrategyFactory _factory;
            private readonly IGitGenerationStrategyFactory _gitGenerationStrategyFactory;

            public Handler(ILogger logger, ISolutionGenerationStrategy solutionGenerationStrategy, ISettingsFileGenerationStrategyFactory factory, IGitGenerationStrategyFactory gitGenerationStrategyFactory)
            {
                _logger = logger ?? throw new ArgumentNullException(nameof(logger));
                _solutionGenerationStrategy = solutionGenerationStrategy ?? throw new ArgumentNullException(nameof(solutionGenerationStrategy));
                _factory = factory ?? throw new ArgumentNullException(nameof(factory));
                _gitGenerationStrategyFactory = gitGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(gitGenerationStrategyFactory));
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _logger.LogInformation($"Handled: {nameof(Microservice)}");

                var options = TinyMapper.Map<CreateCleanArchitectureMicroserviceOptions>(request);

                var model = SolutionModelFactory.CleanArchitectureMicroservice(options);

                _solutionGenerationStrategy.Create(model);

                var settings = SettingsModelFactory.Create(options);

                _factory.CreateFor(settings);

                _gitGenerationStrategyFactory.CreateFor(new GitModel
                {
                    Directory = model.SolutionDirectory,
                    RepositoryName = model.Name
                });

                return new();
            }
        }
    }
}
