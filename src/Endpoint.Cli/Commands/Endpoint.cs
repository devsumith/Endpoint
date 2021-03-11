using CommandLine;
using Endpoint.Cli.Builders;
using Endpoint.Cli.Services;
using MediatR;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using static Endpoint.Cli.Builders.BuilderFactory;

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
            private int _port;
            private string _name;

            public Handler(ICommandService commandService, IMediator mediator)
            {
                _commandService = commandService;
                _mediator = mediator;
            }
            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _port = request.Port;
                _name = request.Name;

                _apiProjectName = $"{request.Name}.Api.csproj";

                _commandService.Start($"mkdir {request.Name}", request.Directory);

                _slnDirectory = $@"{request.Directory}\{request.Name}";

                _commandService.Start($"dotnet new sln -n {request.Name}", _slnDirectory);

                _commandService.Start($"mkdir src", _slnDirectory);

                _commandService.Start($@"mkdir src\{request.Name}.Api", _slnDirectory);

                _apiDirectory = @$"{_slnDirectory}\src\{request.Name}.Api";

                _commandService.Start("dotnet new webapi", _apiDirectory);

                RemoveDefaultTemplateFiles();

                _commandService.Start($@"dotnet sln add {_apiDirectory}\{_apiProjectName}", _slnDirectory);

                await UpdateLaunchSettingsAsync();

                await UpdateStartUpProgramAndDependecies();

                AddNugetPackages();

                _commandService.Start("mkdir Models", _apiDirectory);

                Create((a, b, c, d) => new ModelBuilder(a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/Controllers")
                    .SetRootNamespace(_name)
                    .SetEntityName(request.Resource)
                    .Build();

                _commandService.Start("mkdir Data", _apiDirectory);

                Create((a, b, c, d) => new DbContextBuilder(a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/Data")
                    .SetRootNamespace(_name)
                    .WithModel(request.Resource)
                    .Build();

                _commandService.Start("mkdir Controllers", _apiDirectory);

                Create("cli",(a,b,c,d,e,f) => new ControllerBuilder(a,b,c,d,e,f))
                    .SetResourceName(request.Resource)
                    .SetDirectory($@"{_apiDirectory}/Controllers")
                    .SetRootNamespace(_name)
                    .Build();

                _commandService.Start($"start {request.Name}.sln", $@"{_slnDirectory}");

                _commandService.Start($"dotnet watch run", $@"{_apiDirectory}");

                return new();

            }

            private async Task UpdateLaunchSettingsAsync()
            {

            }

            private async Task UpdateStartUpProgramAndDependecies()
            {

            }

            public void RemoveDefaultTemplateFiles()
            {
                _commandService.Start($"rimraf WeatherForecast.cs", $@"{_apiDirectory}");
                _commandService.Start($@"rimraf Controllers\WeatherForecastController.cs", $@"{_apiDirectory}");
            }

            public void AddNugetPackages()
            {
                _commandService.Start($"dotnet add package Microsoft.EntityFrameworkCore.InMemory", $@"{_apiDirectory}");
                _commandService.Start($"dotnet add package Microsoft.EntityFrameworkCore.SqlServer", $@"{_apiDirectory}");
                _commandService.Start($"dotnet add package FluentValidation", $@"{_apiDirectory}");
                _commandService.Start($"dotnet add package MediatR.Extensions.Microsoft.DependencyInjection", $@"{_apiDirectory}");
                _commandService.Start($"dotnet add package Microsoft.EntityFrameworkCore.Tools", $@"{_apiDirectory}");
                _commandService.Start($"dotnet add package Swashbuckle.AspNetCore", $@"{_apiDirectory}");
                _commandService.Start($"dotnet add package Swashbuckle.AspNetCore.Swagger", $@"{_apiDirectory}");
            }
        }
    }
}
