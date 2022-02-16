using Endpoint.Core.Models;
using Endpoint.Core.Services;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Strategies.Global
{
    public interface IEndpointGenerationStrategyFactory
    {
        void CreateFor(Settings model);
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
            IMediator mediator)
        {
            _strategies = new List<IEndpointGenerationStrategy>()
            {
                new EndpointGenerationStrategy(
                    commandService,
                    solutionFilesGenerationStrategy,
                    sharedKernelProjectFilesGenerationStrategy,
                    applicationProjectFilesGenerationStrategy,
                    infrastructureProjectFilesGenerationStrategy,apiProjectFilesGenerationStrategy,
                    mediator),
                new MinimalApiEndpointGenerationStrategy(commandService,fileSystem,templateLocator, templateProcessor)
            };
        }
        public void CreateFor(Settings model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            var strategy = _strategies.Where(x => x.CanHandle(model)).FirstOrDefault();

            if (strategy == null)
            {
                throw new InvalidOperationException("Cannot find a strategy for generation for the type " + model.SolutionName);
            }

            strategy.Create(model);
        }
    }
}
