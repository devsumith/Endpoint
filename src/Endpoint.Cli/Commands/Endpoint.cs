using CommandLine;
using Endpoint.Cli.Builders;
using Endpoint.Cli.Services;
using Endpoint.Cli.ValueObjects;
using MediatR;
using System;
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
            private string _apiProjectNamespace;
            private string _apiProjectFullPath;
            private string _rootNamespace;
            private string _modelsNamespace;
            private string _dbContextName;

            private int _port;
            private string _name;
            private string _resource;
            private string _directory;

            public Handler(ICommandService commandService, IMediator mediator)
                => _commandService = commandService;

            public async Task<Unit> Handle(Request request, CancellationToken cancellationToken)
            {
                _port = request.Port;
                _name = request.Name;
                _resource = request.Resource;
                _directory = request.Directory;
                _rootNamespace = _name.Replace("-", "_");

                _apiProjectFileName = $"{_name}.Api.csproj";
                _slnDirectory = $@"{_directory}\{_name}";
                _apiDirectory = @$"{_slnDirectory}\src\{_name}.Api";
                _apiProjectFullPath = $@"{_apiDirectory}\{_apiProjectFileName}";
                _apiProjectName = $"{_name}.Api";
                _apiProjectNamespace = $"{_rootNamespace}.Api";
                _modelsNamespace = $"{_rootNamespace}.Api.Models";


                var parts = _name.Split('.');

                _dbContextName = parts.Length > 1 ? $"{parts[1]}DbContext" : $"{parts[0]}DbContext";

                _commandService.Start($"mkdir {request.Name}", _directory);

                _commandService.Start($"dotnet new sln -n {request.Name}", _slnDirectory);

                _commandService.Start($"mkdir src", _slnDirectory);

                _commandService.Start($@"mkdir src\{_apiProjectName}", _slnDirectory);

                _commandService.Start("dotnet new webapi", _apiDirectory);

                _commandService.Start($"dotnet sln add {_apiProjectFullPath}", _slnDirectory);

                RemoveUneededFiles();

                InstallNugetPackages();

                _commandService.Start($"mkdir {Constants.Folders.Models}", _apiDirectory);

                _commandService.Start($"mkdir {Constants.Folders.Data}", _apiDirectory);

                _commandService.Start($"mkdir {Constants.Folders.Core}", _apiDirectory);

                _commandService.Start($"mkdir {Constants.Folders.Behaviors}", _apiDirectory);

                _commandService.Start($"mkdir {Constants.Folders.Extensions}", _apiDirectory);

                _commandService.Start($"mkdir {Constants.Folders.Features}", _apiDirectory);

                Create<LaunchSettingsBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Properties}")
                    .WithPort(_port)
                    .WithSslPort(_port + 1)
                    .WithProjectName(_apiProjectName)
                    .Build();

                Create<ModelBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Models}")
                    .SetNamespace(_modelsNamespace)
                    .SetRootNamespace(_rootNamespace)
                    .SetEntityName(_resource)
                    .Build();

                Create<DbContextBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Data}")
                    .SetRootNamespace(_rootNamespace)
                    .SetNamespace($"{_apiProjectNamespace}.{Constants.Folders.Data}")
                    .WithModel(_resource)
                    .WithDbContextName(_dbContextName)
                    .Build();

                Create<ControllerBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetResource(_resource)
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Controllers}")
                    .SetRootNamespace(_rootNamespace)
                    .Build();

                Create<ResponseBaseBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Core}")
                    .SetRootNamespace(_rootNamespace)
                    .SetNamespace($"{_rootNamespace}.Api.{Constants.Folders.Core}")
                    .Build();

                Create<ValidationBehaviorBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Behaviors}")
                    .SetRootNamespace(_rootNamespace)
                    .SetNamespace($"{_rootNamespace}.Api.{Constants.Folders.Behaviors}")
                    .Build();

                Create<ServiceCollectionExtensionsBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}/{Constants.Folders.Extensions}")
                    .SetRootNamespace(_rootNamespace)
                    .Build();

                Create<ProgramBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}")
                    .SetRootNamespace(_rootNamespace)
                    .SetNamespace(_apiProjectNamespace)
                    .WithDbContextName(_dbContextName)
                    .Build();

                Create<StartupBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}")
                    .SetRootNamespace(_rootNamespace)
                    .SetNamespace(_apiProjectNamespace)
                    .WithDbContextName(_dbContextName)
                    .Build();

                Create<DependenciesBuilder>((a, b, c, d) => new(a, b, c, d))
                    .SetDirectory($@"{_apiDirectory}")
                    .SetRootNamespace(_rootNamespace)
                    .SetNamespace(_apiProjectNamespace)
                    .WithDbContextName(_dbContextName)
                    .Build();

                _commandService.Start($"start {_name}.sln", $@"{_slnDirectory}");

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
