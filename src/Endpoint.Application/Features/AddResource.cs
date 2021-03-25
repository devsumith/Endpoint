using CommandLine;
using Endpoint.Application.Builders;
using Endpoint.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Features
{
    internal class AddResource
    {
        [Verb("add-resource")]
        internal class Request : IRequest<Unit> {
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

            public Handler(ISettingsProvider settingsProvider, IFileSystem fileSystem, ICommandService commandService, ITemplateLocator templateLocator, ITemplateProcessor templateProcessor)
            {
                _settingsProvider = settingsProvider;
                _fileSystem = fileSystem;
                _commandService = commandService;
                _templateProcessor = templateProcessor;
                _templateLocator = templateLocator;
            }
            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvider.Get(request.Directory);

                new ResourceBuilder(_commandService, _templateProcessor, _templateLocator, _fileSystem)
                    .WithSettings(settings)
                    .WithResource(request.Resource)
                    .Build();

                return new();
            }
        }
    }
}
