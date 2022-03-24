using Endpoint.Core.Models;
using Endpoint.Core.Options;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies.Global
{
    public interface IEndpointGenerationStrategyFactory
    {
        void CreateFor(CreateEndpointOptions request);
    }

    public class EndpointGenerationStrategyFactory : IEndpointGenerationStrategyFactory
    {
        private readonly IList<IEndpointGenerationStrategy> _strategies;

        public EndpointGenerationStrategyFactory(
            ICommandService commandService,
            ISolutionFilesGenerationStrategy solutionFilesGenerationStrategy,
            ISharedKernelProjectFilesGenerationStrategy sharedKernelProjectFilesGenerationStrategy,
            IApplicationProjectFilesGenerationStrategy applicationProjectFilesGenerationStrategy,
            IInfrastructureProjectFilesGenerationStrategy infrastructureProjectFilesGenerationStrategy,
            IApiProjectFilesGenerationStrategy apiProjectFilesGenerationStrategy,
            IFileSystem fileSystem,
            ITemplateLocator templateLocator,
            ITemplateProcessor templateProcessor,
            IMediator mediator,
            ISolutionGenerationStrategy solutionGenerationStrategy,
            ILogger logger
            )
        {
            _strategies = new List<IEndpointGenerationStrategy>()
            {
                new EndpointGenerationStrategy(
                    commandService,
                    solutionFilesGenerationStrategy,
                    sharedKernelProjectFilesGenerationStrategy,
                    applicationProjectFilesGenerationStrategy,
                    infrastructureProjectFilesGenerationStrategy,apiProjectFilesGenerationStrategy,
                    mediator,
                    logger),

                new MinimalApiEndpointGenerationStrategy(solutionGenerationStrategy,commandService,fileSystem)
            };
        }

        public void CreateFor(CreateEndpointOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var strategy = _strategies.Where(x => x.CanHandle(options)).OrderByDescending(x => x.Order).FirstOrDefault();

            if (strategy == null)
            {
                throw new InvalidOperationException("Cannot find a strategy for generation for the type " + options.Name);
            }

            strategy.Create(options);
        }
    }
}
