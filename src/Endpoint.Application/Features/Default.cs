using CommandLine;
using Endpoint.Application.Builders;
using Endpoint.Application.Services;
using Endpoint.Application.Services.FileServices;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Features
{
    internal class Default
    {
        [Verb("default")]
        internal class Request : IRequest<Unit>
        {
            [Option('p')]
            public int Port { get; set; } = 5000;

            [Option('n')]
            public string Name { get; set; } = "DefaultEndpoint";

            [Option('r')]
            public string Resource { get; set; } = "Foo";

            [Option('m')]
            public bool Monolith { get; set; } = false;

            [Option('d')]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private ICommandService _commandService;
            private readonly ISolutionFileService _solutionFileService;
            private readonly IDomainFileService _domainFileService;
            private readonly IApplicationFileService _applicationFileService;
            private readonly IInfrastructureFileService _infrastructureFileService;
            private readonly IApiFileService _apiFileService;
            public Handler(
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

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                int retries = 0;

                string name = request.Name;

                while (true)
                {
                    if (!Directory.Exists($"{request.Directory}{Path.DirectorySeparatorChar}{request.Name}"))
                    {
                        var settings = _solutionFileService.Build(request.Name, request.Resource, request.Directory, isMicroserviceArchitecture: !request.Monolith);

                        _domainFileService.Build(settings);

                        _applicationFileService.Build(settings);

                        _infrastructureFileService.Build(settings);

                        _apiFileService.Build(settings);

                        _commandService.Start($"start {settings.SolutionFileName}", settings.RootDirectory);

                        return Task.FromResult(new Unit());
                    }

                    retries++;

                    request.Name = $"{name}_{retries}";

                }
            }
        }
    }
}
