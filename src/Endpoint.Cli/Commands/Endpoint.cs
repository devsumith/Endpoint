using CommandLine;
using Endpoint.Cli.Services;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands
{
    internal class Endpoint
    {
        [Verb("Default")]
        internal class Request : IRequest<Unit> {
            [Option('p')]
            public int Port { get; set; } = 5000;

            [Option('n')]
            public string Name { get; set; } = $"DefaultEndpoint-{Guid.NewGuid()}";

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
            private readonly IMediator _mediator;
            private string _apiDirectory;
            private string _slnDirectory;
            private string _apiProjectName;

            public Handler(ICommandService commandService, IMediator mediator)
            {
                _commandService = commandService;
                _mediator = mediator;
            }
            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {

                _apiProjectName = $"{request.Name}.Api.csproj";

                _commandService.Start($"mkdir {request.Name}", request.Directory);

                _slnDirectory = $@"{request.Directory}\{request.Name}";

                _commandService.Start($"dotnet new sln -n {request.Name}", _slnDirectory);

                _commandService.Start($"mkdir src", _slnDirectory);

                _commandService.Start($@"mkdir src\{request.Name}.Api", _slnDirectory);

                _apiDirectory = @$"{_slnDirectory}\src\{request.Name}.Api";

                _commandService.Start("dotnet new webapi", _apiDirectory);

                _commandService.Start($@"dotnet sln add {_apiDirectory}\{_apiProjectName}", _slnDirectory);

                _commandService.Start($"start {request.Name}.sln", $@"{_slnDirectory}");

                RemoveDefaultTemplateFiles();

                await _mediator.Send(new LaunchSettings.Request { Directory = $@"{_apiDirectory}\Properties", Port = request.Port, SslPort = request.Port + 1, Name = request.Name }, cancellationToken);

                _commandService.Start($@"dotnet sln add {_apiDirectory}\{_apiProjectName}", _slnDirectory);

                _commandService.Start($"start {request.Name}.sln", $@"{_slnDirectory}");

                return new();

            }

            public void RemoveDefaultTemplateFiles()
            {
                _commandService.Start($"rimraf WeatherForecast.cs", $@"{_apiDirectory}");
                _commandService.Start($@"rimraf Controllers\WeatherForecastController.cs", $@"{_apiDirectory}");
            }
        }
    }
}
