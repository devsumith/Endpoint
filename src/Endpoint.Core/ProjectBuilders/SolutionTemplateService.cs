using Endpoint.Core.Services;
using Endpoint.SharedKernal.Events;
using Endpoint.SharedKernal.Services;
using MediatR;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Core.Core.Services
{
    public interface ISolutionTemplateService
    {
        void Build(string name, string dbContextName, bool shortIdPropertyName, string resource, string properties, bool isMonolith, bool numericIdPropertyDataType, string directory, List<string> plugins);
    }

    public class SolutionTemplateService : ISolutionTemplateService
    {
        private ICommandService _commandService;
        private readonly ISolutionFileService _solutionFileService;
        private readonly IDomainFileService _domainFileService;
        private readonly IApplicationFileService _applicationFileService;
        private readonly IInfrastructureFileService _infrastructureFileService;
        private readonly IApiFileService _apiFileService;
        private readonly IMediator _mediator;

        public SolutionTemplateService(
            ICommandService commandService,
            ISolutionFileService solutionFileService,
            IDomainFileService domainFileService,
            IApplicationFileService applicationFileService,
            IInfrastructureFileService infrastructureFileService,
            IApiFileService apiFileService,
            IMediator mediator)
        {
            _commandService = commandService;
            _solutionFileService = solutionFileService;
            _domainFileService = domainFileService;
            _applicationFileService = applicationFileService;
            _infrastructureFileService = infrastructureFileService;
            _apiFileService = apiFileService;
            _mediator = mediator;
        }

        public void Build(string name, string dbContextName, bool shortIdPropertyName, string resource, string properties, bool isMonolith, bool numericIdPropertyDataType, string directory, List<string> plugins)
        {
            Build(name, dbContextName, shortIdPropertyName, new List<string>() { resource }, properties, isMonolith, numericIdPropertyDataType, directory, plugins);
        }

        public void Build(string name, string dbContextName, bool shortIdPropertyName, List<string> resources, string properties, bool isMonolith, bool numericIdPropertyDataType, string directory, List<string> plugins)
        {
            int retries = 0;

            string originalName = name;

            while (true)
            {
                if (!Directory.Exists($"{directory}{Path.DirectorySeparatorChar}{name}"))
                {
                    var settings = _solutionFileService.Build(name, properties, dbContextName, shortIdPropertyName, numericIdPropertyDataType, resources, directory, isMicroserviceArchitecture: !isMonolith, plugins);

                    _domainFileService.Build(settings);

                    _applicationFileService.Build(settings);

                    _infrastructureFileService.Build(settings);

                    _apiFileService.Build(settings);

                    _mediator.Publish(new SolutionTemplateGenerated(settings.RootDirectory));

                    _commandService.Start($"start {settings.SolutionFileName}", settings.RootDirectory);

                    return;

                }

                retries++;

                name = $"{originalName}_{retries}";

            }
        }
    }
}
