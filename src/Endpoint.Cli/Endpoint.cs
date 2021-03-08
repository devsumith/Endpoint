using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli
{
    internal class Endpoint
    {
        [Verb("Default")]
        internal class Request : IRequest<Unit> {
            [Option('p')]
            public int Port { get; set; } = 5001;

            [Option('n')]
            public string Name { get; set; } = "DefaultEndpoint";

            [Option('r')]
            public string Resource { get; set; } = "Foo";

            [Option('m')]
            public string Method { get; set; } = "Get";

            [Option('d')]
            public string Directory { get; set; } = System.Environment.CurrentDirectory;
        }

        internal class Handler : IRequestHandler<Request, Unit>
        {
            private readonly ICommandService _commandService;

            public Handler(ICommandService commandService)
            {
                _commandService = commandService;
            }
            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _commandService.Start($"rimraf {request.Name}", request.Directory);

                _commandService.Start($"mkdir {request.Name}", request.Directory);

                var slnDirectory = $@"{request.Directory}\{request.Name}";

                _commandService.Start($"dotnet new sln -n {request.Name}", slnDirectory);

                _commandService.Start($"mkdir src", slnDirectory);

                _commandService.Start($@"mkdir src\{request.Name}", slnDirectory);

                var apiDirectory = @$"{slnDirectory}\src\{request.Name}";

                _commandService.Start("dotnet new webapi", apiDirectory);

                _commandService.Start($@"dotnet sln add {apiDirectory}\{request.Name}.csproj", slnDirectory);

                _commandService.Start($"start {request.Name}.sln", $@"{slnDirectory}");

                return new();
            }
        }
    }
}
