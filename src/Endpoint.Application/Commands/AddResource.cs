using CommandLine;
using Endpoint.Core.Services;
using Endpoint.Core.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands
{
    internal class AddResource
    {
        [Verb("add-resource")]
        internal class Request : IRequest<Unit>
        {
            [Value(0)]
            public string Resource { get; set; }
            [Option('d')]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;

        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ISettingsProvider _settingsProvider;
            private readonly IFileSystem _fileSystem;
            private readonly ICommandService _commandService;
            private readonly ITemplateProcessor _templateProcessor;
            private readonly ITemplateLocator _templateLocator;
            private readonly IApplicationProjectFilesGenerationStrategy _applicationFileService;
            private readonly IInfrastructureProjectFilesGenerationStrategy _infrastructureFileService;
            private readonly IApiProjectFilesGenerationStrategy _apiFileService;

            public Handler(
                ISettingsProvider settingsProvider,
                IFileSystem fileSystem,
                ICommandService commandService,
                ITemplateLocator templateLocator,
                ITemplateProcessor templateProcessor,
                IApplicationProjectFilesGenerationStrategy applicationFileService,
                IInfrastructureProjectFilesGenerationStrategy infrastructureFileService,
                IApiProjectFilesGenerationStrategy apiFileService)
            {
                _settingsProvider = settingsProvider;
                _fileSystem = fileSystem;
                _commandService = commandService;
                _templateProcessor = templateProcessor;
                _templateLocator = templateLocator;
                _applicationFileService = applicationFileService;
                _infrastructureFileService = infrastructureFileService;
                _apiFileService = apiFileService;
            }
            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvider.Get(request.Directory);

                settings.AddResource(request.Resource, _fileSystem);

                _applicationFileService.BuildAdditionalResource(request.Resource, settings);

                _infrastructureFileService.BuildAdditionalResource(request.Resource, settings);

                _apiFileService.BuildAdditionalResource(request.Resource, settings);

                return Task.FromResult(new Unit());
            }
        }
    }
}
