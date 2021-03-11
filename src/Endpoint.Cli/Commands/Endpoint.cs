using CommandLine;
using Endpoint.Cli.Builders;
using Endpoint.Cli.Services;
using Endpoint.Cli.ValueObjects;
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
            private string _apiDirectory;
            private string _slnDirectory;

            private string _apiProjectFileName;
            private string _apiProjectName;

            private string _modelsNamespace;


            private int _port;
            private string _name;

            public Handler(ICommandService commandService, IMediator mediator)
                => _commandService = commandService;

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _port = request.Port;
                _name = request.Name;
                _apiProjectFileName = $"{request.Name}.Api.csproj";                
                _slnDirectory = $@"{request.Directory}\{request.Name}";                
                _apiDirectory = @$"{_slnDirectory}\src\{request.Name}.Api";
                _apiProjectName = $"{request.Name}.Api";
                _modelsNamespace = $"{request.Name}.Api.Models";

                _commandService.Start($"mkdir {request.Name}", request.Directory);

                _commandService.Start($"dotnet new sln -n {request.Name}", _slnDirectory);

                _commandService.Start($"mkdir src", _slnDirectory);

                _commandService.Start($@"mkdir src\{_apiProjectName}", _slnDirectory);

                _commandService.Start("dotnet new webapi", _apiDirectory);

                _commandService.Start($@"dotnet sln add {_apiDirectory}\{_apiProjectFileName}", _slnDirectory);

                RemoveUneededFiles();

                InstallNugetPackages();

                _commandService.Start($"mkdir {Constants.Folders.Models}", _apiDirectory);

                _commandService.Start($"mkdir {Constants.Folders.Data}", _apiDirectory);

                _commandService.Start($"mkdir {Constants.Folders.Controllers}", _apiDirectory);

                _commandService.Start($"mkdir {Constants.Folders.Core}", _apiDirectory);

                _commandService.Start($"mkdir {Constants.Folders.Behaviors}", _apiDirectory);

                _commandService.Start($"mkdir {Constants.Folders.Extensions}", _apiDirectory);

                _commandService.Start($"mkdir {Constants.Folders.Features}", _apiDirectory);

                Create<ModelBuilder>((a, b, c, d) => new (a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Models}")
                    .SetNamespace(_modelsNamespace)
                    .SetRootNamespace(_name)
                    .SetEntityName(request.Resource)
                    .Build();

                Create<DbContextBuilder>((a, b, c, d) => new (a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Data}")
                    .SetRootNamespace(_name)
                    .WithModel(request.Resource)
                    .Build();

                Create<ControllerBuilder>("cli",(a,b,c,d,e,f) => new (a,b,c,d,e,f))
                    .SetResourceName(request.Resource)
                    .SetDirectory($@"{_apiDirectory}/Controllers")
                    .SetRootNamespace(_name)
                    .Build();

                Create<ResponseBaseBuilder>((a, b, c, d) => new (a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Core}")
                    .SetRootNamespace(_name)
                    .Build();

                Create<ValidationBehaviorBuilder>((a, b, c, d) => new (a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/Behaviors")
                    .SetRootNamespace(_name)
                    .Build();

                Create<ServiceCollectionExtensionsBuilder>((a, b, c, d) => new (a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Extensions}")
                    .SetRootNamespace(_name)
                    .Build();

                Create<QueryBuilder>((a, b, c, d) => new (a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Features}")
                    .SetRootNamespace(_name)
                    .WithName($"Get{((Token)request.Resource).PascalCasePlural}")
                    .WithEntity(request.Resource)
                    .Build();

                Create<ProgramBuilder>((a, b, c, d) => new (a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Features}")
                    .SetRootNamespace(_name)
                    .Build();

                Create<StartupBuilder>((a, b, c, d) => new (a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Features}")
                    .SetRootNamespace(_name)
                    .Build();

                _commandService.Start($"start {request.Name}.sln", $@"{_slnDirectory}");

                _commandService.Start($"dotnet watch run", $@"{_apiDirectory}");

                return new();

            }

            public void RemoveUneededFiles()
            {
                _commandService.Start($"rimraf WeatherForecast.cs", $@"{_apiDirectory}");
                _commandService.Start($@"rimraf Controllers\WeatherForecastController.cs", $@"{_apiDirectory}");
            }

            public void InstallNugetPackages()
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
