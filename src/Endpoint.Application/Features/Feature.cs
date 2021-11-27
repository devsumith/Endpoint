using CommandLine;
using Endpoint.Application.Services;
using Endpoint.Application.ValueObjects;
using MediatR;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Application.Features
{
    internal class Feature
    {
        [Verb("feature")]
        internal class Request : IRequest<Unit>
        {
            [Value(0)]
            public string Entity { get; set; }
            [Option('d')]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ICommandService _commandService;
            private readonly ISettingsProvider _settingsProvider;

            public Handler(ICommandService commandService, ISettingsProvider settingsProvider)
            {
                _commandService = commandService;
                _settingsProvider = settingsProvider;
            }
            public Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                if (request.Directory.EndsWith("Features"))
                {
                    _commandService.Start($"mkdir {((Token)request.Entity).PascalCasePlural}", request.Directory);

                    request.Directory = $"{request.Directory}{Path.DirectorySeparatorChar}{((Token)request.Entity).PascalCasePlural}";
                }

                _commandService.Start($"endpoint command Create{request.Entity} {request.Entity}", request.Directory);
                _commandService.Start($"endpoint command Update{request.Entity} {request.Entity}", request.Directory);
                _commandService.Start($"endpoint command Delete{request.Entity} {request.Entity}", request.Directory);
                _commandService.Start($"endpoint query Get{((Token)request.Entity).PascalCasePlural} {request.Entity}", request.Directory);
                _commandService.Start($"endpoint query Get{request.Entity}ById {request.Entity}", request.Directory);
                _commandService.Start($"endpoint dto {request.Entity}", request.Directory);
                _commandService.Start($"endpoint validator {request.Entity}", request.Directory);
                _commandService.Start($"endpoint extensions {request.Entity}", request.Directory);

                return Task.FromResult(new Unit());
            }
        }
    }
}
