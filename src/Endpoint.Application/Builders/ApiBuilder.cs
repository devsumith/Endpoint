using Endpoint.Application.Models;
using Endpoint.Application.Services;
using System.IO;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.Application.Builders
{
    public class ApiBuilder : BuilderBase<ApiBuilder>
    {
        public ApiBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem) : base(commandService, templateProcessor, templateLocator, fileSystem)
        { }

        protected new string _apiDirectory => $"{_directory.Value}{Path.DirectorySeparatorChar}{_name}";

        protected string _apiProjectFullPath => $"{_apiDirectory}{Path.DirectorySeparatorChar}{_apiProjectName}.csproj";

        protected int _port;
        protected int _sslPort;
        protected string _apiProjectName;
        protected string _modelsNamespace;
        protected string _resource;
        protected string _apiProjectNamespace;
        protected string _dbContextName;
        protected string _store;
        protected string _name;

        public ApiBuilder WithPort(int port)
        {
            _port = port;
            return this;
        }
        public ApiBuilder WithSslPort(int sslPort)
        {
            _sslPort = sslPort;
            return this;
        }
        public ApiBuilder WithName(string name)
        {
            _apiProjectName = name;
            _name = name;
            return this;
        }
        public ApiBuilder WithModelsNamespace(string modelsNamespace)
        {
            _modelsNamespace = modelsNamespace;
            return this;
        }
        public ApiBuilder WithResource(string resource)
        {
            _resource = resource;
            return this;
        }
        public ApiBuilder WithApiProjectNamespace(string apiProjectNamespace)
        {
            _apiProjectNamespace = apiProjectNamespace;
            return this;
        }
        public ApiBuilder WithDbContext(string dbContext)
        {
            _dbContextName = dbContext;
            return this;
        }
        public ApiBuilder WithStore(string store)
        {
            _store = store;
            return this;
        }

        public ProjectReference Build()
        {
            _commandService.Start($@"mkdir {_name}", _directory);

            _commandService.Start("dotnet new webapi --framework net5.0", _apiDirectory);

            RemoveUneededFiles();

            InstallNugetPackages();

            CreateSubDirectories();

            Create<LaunchSettingsBuilder>((a, b, c, d) => new(a, b, c, d))
                .SetDirectory($@"{_apiDirectory}{Path.DirectorySeparatorChar}{Constants.Folders.Properties}")
                .WithPort(_port)
                .WithSslPort(_port + 1)
                .WithProjectName(_apiProjectName)
                .Build();

            Create<ResponseBaseBuilder>((a, b, c, d) => new(a, b, c, d))
                .SetDirectory($@"{_domainDirectory.Value}{Path.DirectorySeparatorChar}{Constants.Folders.Core}")
                .SetRootNamespace(_domainNamespace.Value)
                .SetNamespace($"{(_domainNamespace.Value)}.{Constants.Folders.Core}")
                .Build();

            Create<ValidationBehaviorBuilder>((a, b, c, d) => new(a, b, c, d))
                .SetDirectory($@"{_applicationDirectory.Value}{Path.DirectorySeparatorChar}{Constants.Folders.Behaviors}")
                .SetApplicationNamespace(_applicationNamespace.Value)
                .Build();

            Create<ServiceCollectionExtensionsBuilder>((a, b, c, d) => new(a, b, c, d))
                .SetDirectory($@"{_applicationDirectory.Value}{Path.DirectorySeparatorChar}{Constants.Folders.Extensions}")
                .SetApplicationNamespace(_applicationNamespace.Value)
                .Build();

            Create<QueryableExtensionsBuilder>((a, b, c, d) => new(a, b, c, d))
                .SetDirectory($@"{_applicationDirectory.Value}{Path.DirectorySeparatorChar}{Constants.Folders.Extensions}")
                .SetApplicationNamespace(_applicationNamespace.Value)
                .Build();

            Create<ProgramBuilder>((a, b, c, d) => new(a, b, c, d))
                .SetDirectory($@"{_apiDirectory}")
                .SetRootNamespace(_rootNamespace.Value)
                .SetInfrastructureNamespace(_infrastructureNamespace.Value)
                .SetNamespace(_apiProjectNamespace)
                .WithDbContextName(_dbContextName)
                .Build();

            Create<AppSettingsBuilder>((a, b, c, d) => new(a, b, c, d))
                .SetDirectory($@"{_apiDirectory}")
                .SetRootNamespace(_rootNamespace.Value)
                .SetInfrastructureNamespace(_infrastructureNamespace.Value)
                .SetNamespace(_apiProjectNamespace)
                .Build();

            Create<StartupBuilder>((a, b, c, d) => new(a, b, c, d))
                .SetDirectory($@"{_apiDirectory}")
                .SetRootNamespace(_apiNamespace.Value)
                .SetNamespace(_apiProjectNamespace)
                .WithDbContextName(_dbContextName)
                .Build();

            Create<DependenciesBuilder>((a, b, c, d) => new(a, b, c, d))
                .SetDirectory($@"{_apiDirectory}")
                .SetRootNamespace(_apiNamespace.Value)
                .SetNamespace(_apiProjectNamespace)
                .SetDomainNamespace(_domainNamespace.Value)
                .SetInfrastructureNamespace(_infrastructureNamespace.Value)
                .SetApplicationNamespace(_applicationNamespace.Value)
                .SetApiNamespace(_apiProjectNamespace)
                .WithDbContextName(_dbContextName)
                .Build();

            Create<SeedDataBuilder>((a, b, c, d) => new(a, b, c, d))
                .SetInfrastructureDirectory($@"{_infrastructureDirectory.Value}")
                .SetInfrastructureNamespace(_infrastructureNamespace.Value)
                .WithDbContext(_dbContextName)
                .Build();


            return new(_commandService, _apiDirectory, _apiProjectFullPath, $"{_rootNamespace.Value}.Api");
        }

        private void CreateSubDirectories()
        {
            _commandService.Start($"mkdir {Constants.Folders.Models}", _domainDirectory.Value);

            _commandService.Start($"mkdir {Constants.Folders.Data}", _infrastructureDirectory.Value);

            _commandService.Start($"mkdir {Constants.Folders.Core}", _domainDirectory.Value);

            _commandService.Start($"mkdir {Constants.Folders.Behaviors}", _applicationDirectory.Value);

            _commandService.Start($"mkdir {Constants.Folders.Extensions}", _applicationDirectory.Value);

            _commandService.Start($"mkdir {Constants.Folders.Features}", _applicationDirectory.Value);

            _commandService.Start($"mkdir {Constants.Folders.Interfaces}", _applicationDirectory.Value);

            _commandService.Start($"mkdir {Constants.Folders.Logs}", _apiDirectory);

        }

        public void RemoveUneededFiles()
        {
            _commandService.Start($"rimraf WeatherForecast.cs", $@"{_apiDirectory}");
            _commandService.Start($@"rimraf Controllers\WeatherForecastController.cs", $@"{_apiDirectory}");
        }

        public void InstallNugetPackages()
        {
            _commandService.Start($"dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 5.0.10", $@"{_apiDirectory}");
            _commandService.Start($"dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.10", $@"{_apiDirectory}");
            _commandService.Start($"dotnet add package FluentValidation --version 10.3.3", $@"{_apiDirectory}");
            _commandService.Start($"dotnet add package MediatR.Extensions.Microsoft.DependencyInjection  --version 9.0.0", $@"{_apiDirectory}");
            _commandService.Start($"dotnet add package Microsoft.EntityFrameworkCore.Tools --version 5.0.10", $@"{_apiDirectory}");
            _commandService.Start($"dotnet add package Swashbuckle.AspNetCore --version 6.2.2", $@"{_apiDirectory}");
            _commandService.Start($"dotnet add package Swashbuckle.AspNetCore.Swagger --version 6.2.2", $@"{_apiDirectory}");
            _commandService.Start($"dotnet add package Newtonsoft.Json", $@"{_apiDirectory}");
            _commandService.Start($"dotnet add package Serilog.AspNetCore --version 4.1.0", $@"{_apiDirectory}");
            _commandService.Start($"dotnet add package Serilog.Sinks.Seq --version 5.1.1", $@"{_apiDirectory}");
            _commandService.Start($"dotnet add package SerilogTimings --version 2.3.0", $@"{_apiDirectory}");
        }
    }
}
