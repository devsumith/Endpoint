using Endpoint.Core.Events;
using Endpoint.Core.Models;
using Endpoint.Core.Options;
using Endpoint.Core.Strategies.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Services
{
    public class EndpointGenerationStrategy : ISolutionTemplateService, IEndpointGenerationStrategy
    {
        private ICommandService _commandService;
        private readonly ISolutionFilesGenerationStrategy _solutionFilesGenerationStrategy;
        private readonly ISharedKernelProjectFilesGenerationStrategy _sharedKernelFilesGenerationStrategy;
        private readonly IApplicationProjectFilesGenerationStrategy _applicationFilesGenerationStrategy;
        private readonly IInfrastructureProjectFilesGenerationStrategy _infrastructureFilesGenerationStrategy;
        private readonly IApiProjectFilesGenerationStrategy _apiProjectFilesGenerationStrategy;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public EndpointGenerationStrategy(
            ICommandService commandService,
            ISolutionFilesGenerationStrategy solutionFileService,
            ISharedKernelProjectFilesGenerationStrategy domainFileService,
            IApplicationProjectFilesGenerationStrategy applicationFileService,
            IInfrastructureProjectFilesGenerationStrategy infrastructureFileService,
            IApiProjectFilesGenerationStrategy apiFileService,
            IMediator mediator,
            ILogger logger)
        {
            _commandService = commandService;
            _solutionFilesGenerationStrategy = solutionFileService;
            _sharedKernelFilesGenerationStrategy = domainFileService;
            _applicationFilesGenerationStrategy = applicationFileService;
            _infrastructureFilesGenerationStrategy = infrastructureFileService;
            _apiProjectFilesGenerationStrategy = apiFileService;
            _mediator = mediator;
            _logger = logger;
        }

        public int Order => 0;
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

        public bool CanHandle(CreateEndpointOptions request) => !request.Minimal.Value;

        public void Create(CreateEndpointOptions request)
        {
            var model = new Settings(
                        request.Name,
                        request.DbContextName,
                        new AggregateRootModel(request.Resource, request.NumericIdPropertyDataType.Value, request.ShortIdPropertyName.Value, request.Properties),
                        request.Directory,
                        !request.Monolith.Value,
                        default,
                        request.ShortIdPropertyName.Value ? IdFormat.Short : IdFormat.Long,
                        request.NumericIdPropertyDataType.Value ? IdDotNetType.Int : IdDotNetType.Guid,
                        default,
                        request.Minimal.Value);

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
