using Endpoint.Core.Events;
using Endpoint.Core.Strategies.Global;
using MediatR;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Services
{
    public interface ISolutionTemplateService
    {
        void Build(string name, string dbContextName, bool shortIdPropertyName, string resource, string properties, bool isMonolith, bool numericIdPropertyDataType, string directory, List<string> plugins, string prefix);
    }

    public class EndpointGenerationStrategy : ISolutionTemplateService, IEndpointGenerationStrategy
    {
        private ICommandService _commandService;
        private readonly ISolutionFilesGenerationStrategy _solutionFilesGenerationStrategy;
        private readonly ISharedKernelProjectFilesGenerationStrategy _sharedKernelFilesGenerationStrategy;
        private readonly IApplicationProjectFilesGenerationStrategy _applicationFilesGenerationStrategy;
        private readonly IInfrastructureProjectFilesGenerationStrategy _infrastructureFilesGenerationStrategy;
        private readonly IApiProjectFilesGenerationStrategy _apiProjectFilesGenerationStrategy;
        private readonly IMediator _mediator;

        public EndpointGenerationStrategy(
            ICommandService commandService,
            ISolutionFilesGenerationStrategy solutionFileService,
            ISharedKernelProjectFilesGenerationStrategy domainFileService,
            IApplicationProjectFilesGenerationStrategy applicationFileService,
            IInfrastructureProjectFilesGenerationStrategy infrastructureFileService,
            IApiProjectFilesGenerationStrategy apiFileService,
            IMediator mediator)
        {
            _commandService = commandService;
            _solutionFilesGenerationStrategy = solutionFileService;
            _sharedKernelFilesGenerationStrategy = domainFileService;
            _applicationFilesGenerationStrategy = applicationFileService;
            _infrastructureFilesGenerationStrategy = infrastructureFileService;
            _apiProjectFilesGenerationStrategy = apiFileService;
            _mediator = mediator;
        }

        public void Build(string name, string dbContextName, bool shortIdPropertyName, string resource, string properties, bool isMonolith, bool numericIdPropertyDataType, string directory, List<string> plugins, string prefix)
        {
            Build(name, dbContextName, shortIdPropertyName, new List<string>() { resource }, properties, isMonolith, numericIdPropertyDataType, directory, plugins, prefix);
        }

        public void Build(string name, string dbContextName, bool shortIdPropertyName, List<string> resources, string properties, bool isMonolith, bool numericIdPropertyDataType, string directory, List<string> plugins, string prefix)
        {
            int retries = 0;

            string originalName = name;

            while (true)
            {
                if (!Directory.Exists($"{directory}{Path.DirectorySeparatorChar}{name}"))
                {
                    var settings = _solutionFilesGenerationStrategy.Build(name, properties, dbContextName, shortIdPropertyName, numericIdPropertyDataType, resources, directory, isMicroserviceArchitecture: !isMonolith, plugins, prefix);

                    _sharedKernelFilesGenerationStrategy.Build(settings);

                    _applicationFilesGenerationStrategy.Build(settings);

                    _infrastructureFilesGenerationStrategy.Build(settings);

                    _apiProjectFilesGenerationStrategy.Build(settings);

                    _mediator.Publish(new SolutionTemplateGenerated(settings.RootDirectory));

                    _commandService.Start($"start {settings.SolutionFileName}", settings.RootDirectory);

                    return;

                }

                retries++;

                name = $"{originalName}_{retries}";

            }
        }

        public bool CanHandle(Models.Settings model)
        {
            return !model.Minimal;
        }

        public void Create(Models.Settings model)
        {
            _solutionFilesGenerationStrategy.Create(model);

            _sharedKernelFilesGenerationStrategy.Build(model);

            _applicationFilesGenerationStrategy.Build(model);

            _infrastructureFilesGenerationStrategy.Build(model);

            _apiProjectFilesGenerationStrategy.Build(model);

            _mediator.Publish(new SolutionTemplateGenerated(model.RootDirectory));

            _commandService.Start($"start {model.SolutionFileName}", model.RootDirectory);
        }
    }
}
