using CommandLine;
using Endpoint.Application.Builders;
using Endpoint.SharedKernal.Services;
using Endpoint.SharedKernal.ValueObjects;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;


namespace Endpoint.Application.Features
{
    internal class Command
    {
        [Verb("command")]
        internal class Request : IRequest<Unit>
        {

            [Value(0)]
            public string Name { get; set; }

            [Value(1)]
            public string Entity { get; set; }

            [Option('d')]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ISettingsProvider _settingsProvder;
            private readonly IFileSystem _fileSystem;

            public Handler(ISettingsProvider settingsProvider, IFileSystem fileSystem)
            {
                _settingsProvder = settingsProvider ?? throw new ArgumentNullException(nameof(settingsProvider));
                _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            }

            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                var settings = _settingsProvder.Get(request.Directory);

                CommandBuilder.Build(settings, (Token)request.Name, new Context(), _fileSystem, request.Directory, settings.ApplicationNamespace);

                return Task.FromResult(new Unit());
            }
        }
    }
}
