using CommandLine;
using Endpoint.Application.Builders;
using Endpoint.Application.Services;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Features
{
    internal class DomainEvent
    {
        [Verb("domain-event")]
        internal class Request : IRequest<Unit>
        {
            [Option('d', Required = false)]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;

            [Value(0)]
            public string Entity { get; set; }
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ISettingsProvider _settingsProvider;
            private readonly ICommandService _commandService;
            private readonly ITemplateLocator _templateLocator;
            private readonly IFileSystem _fileSystem;
            private readonly ITemplateProcessor _templateProcessor;

            public Handler(
                ISettingsProvider settingsProvider,
                ICommandService commandService,
                ITemplateLocator templateLocator,
                IFileSystem fileSystem,
                ITemplateProcessor templateProcessor
                )
            {
                _settingsProvider = settingsProvider;
                _commandService = commandService;
                _templateLocator = templateLocator;
                _templateProcessor = templateProcessor;
                _fileSystem = fileSystem;
            }

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvider.Get(request.Directory);

                new DomainEventBuilder(
                    new Context(),
                    _commandService,
                    _templateProcessor,
                    _templateLocator,
                    _fileSystem)
                    .SetDomainDirectory(settings.DomainDirectory)
                    .SetDomainNamespace(settings.DomainNamespace)
                    .SetEntityName(request.Entity)
                    .Build();

                return new();
            }
        }
    }
}
