using CommandLine;
using Endpoint.Core.Builders;
using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
using MediatR;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Commands
{
    internal class Event
    {
        [Verb("event")]
        internal class Request : IRequest<Unit>
        {
            [Value(0)]
            public string Aggregate { get; set; }
            [Value(1)]
            public string Name { get; set; }
            [Option('d', Required = false)]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly IFileSystem _fileSystem;
            private readonly ITemplateLocator _templateLocator;
            private readonly ITemplateProcessor _templateProcessor;
            private readonly ICommandService _commandService;
            private readonly ISettingsProvider _settingsProvider;

            public Handler(
                IFileSystem fileSystem,
                ITemplateLocator templateLocator,
                ITemplateProcessor templateProcessor,
                ICommandService commandService,
                ISettingsProvider settingsProvider
                )
            {
                _fileSystem = fileSystem;
                _templateProcessor = templateProcessor;
                _templateLocator = templateLocator;
                _commandService = commandService;
                _settingsProvider = settingsProvider;
            }

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvider.Get(request.Directory);

                /*                BuilderFactory.Create<EventBuilder>((a, b, c, d) => new(a, b, c, d))
                                    .WithAggregate(request.Aggregate)
                                    .WithEvent(request.Name)
                                    .SetDomainDirectory(settings.DomainDirectory)
                                    .SetDomainNamespace(settings.DomainNamespace)
                                    .Build();*/

                return Task.FromResult(new Unit());
            }
        }
    }
}
