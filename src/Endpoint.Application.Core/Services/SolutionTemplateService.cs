using Endpoint.Application.Services;
using Endpoint.SharedKernal.Services;
using System.Collections.Generic;
using System.IO;

namespace Endpoint.Application.Core.Services
{
    public interface ISolutionTemplateService
    {
        void Build(string name, string dbContextName, bool shortIdPropertyName, string resource, bool isMonolith, bool numericIdPropertyDataType, string directory);
    }

    public class SolutionTemplateService: ISolutionTemplateService
    {
        private ICommandService _commandService;
        private readonly ISolutionFileService _solutionFileService;
        private readonly IDomainFileService _domainFileService;
        private readonly IApplicationFileService _applicationFileService;
        private readonly IInfrastructureFileService _infrastructureFileService;
        private readonly IApiFileService _apiFileService;
        public SolutionTemplateService(
            ICommandService commandService,
            ISolutionFileService solutionFileService,
            IDomainFileService domainFileService,
            IApplicationFileService applicationFileService,
            IInfrastructureFileService infrastructureFileService,
            IApiFileService apiFileService)
        {
            _commandService = commandService;
            _solutionFileService = solutionFileService;
            _domainFileService = domainFileService;
            _applicationFileService = applicationFileService;
            _infrastructureFileService = infrastructureFileService;
            _apiFileService = apiFileService;
        }

        public void Build(string name, string dbContextName, bool shortIdPropertyName, string resource, bool isMonolith, bool numericIdPropertyDataType, string directory)
        {
            int retries = 0;

            string originalName = name;

            while (true)
            {
                if (!Directory.Exists($"{directory}{Path.DirectorySeparatorChar}{name}"))
                {
                    var settings = _solutionFileService.Build(name, dbContextName, shortIdPropertyName, numericIdPropertyDataType, resource, directory, isMicroserviceArchitecture: !isMonolith);

                    _domainFileService.Build(settings);

                    _applicationFileService.Build(settings);

                    _infrastructureFileService.Build(settings);

                    _apiFileService.Build(settings);

                    _commandService.Start($"start {settings.SolutionFileName}", settings.RootDirectory);

                    return;

                }

                retries++;

                name = $"{originalName}_{retries}";

            }
        }

        public void Build(string name, string dbContextName, bool shortIdPropertyName, List<string> resources, bool isMonolith, bool numericIdPropertyDataType, string directory)
        {
            int retries = 0;

            string originalName = name;

            while (true)
            {
                if (!Directory.Exists($"{directory}{Path.DirectorySeparatorChar}{name}"))
                {
                    var settings = _solutionFileService.Build(name, dbContextName, shortIdPropertyName, numericIdPropertyDataType, resources, directory, isMicroserviceArchitecture: !isMonolith);

                    _domainFileService.Build(settings);

                    _applicationFileService.Build(settings);

                    _infrastructureFileService.Build(settings);

                    _apiFileService.Build(settings);

                    _commandService.Start($"start {settings.SolutionFileName}", settings.RootDirectory);

                    return;

                }

                retries++;

                name = $"{originalName}_{retries}";

            }
        }
    }
}
