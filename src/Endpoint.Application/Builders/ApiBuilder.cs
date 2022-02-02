/*using Endpoint.Application.Models;
using Endpoint.Application.Services;
using System.IO;
using static Endpoint.Application.Builders.BuilderFactory;

namespace Endpoint.Application.Builders
{
    public class ApiBuilder
    {
        private readonly ICommandService _commandService;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly ITemplateLocator _templateLocator;
        private readonly IFileSystem _fileSystem;
        private readonly ILaunchSettingsBuilder _launchSettingsBuilder;
        private readonly IResponseBaseBuilder _responseBaseBuilder;

        public ApiBuilder(
            ICommandService commandService,
            ITemplateProcessor templateProcessor,
            ITemplateLocator templateLocator,
            IFileSystem fileSystem)
        {
            _commandService = commandService;
            _templateProcessor = templateProcessor;
            _templateLocator = templateLocator;
            _fileSystem = fileSystem;
        }

        public ProjectReference Build(Models.Settings settings)
        {
            _commandService.Start($@"mkdir {settings.ApiDirectory}");

            _commandService.Start("dotnet new webapi --framework net5.0", settings.ApiDirectory);

            _commandService.Start("dotnet new tool-manifest", settings.ApiDirectory);

            _commandService.Start("dotnet tool install --version 6.0.7 Swashbuckle.AspNetCore.Cli", settings.ApiDirectory);

            RemoveUneededFiles(settings);

            InstallNugetPackages(settings);

            CreateSubDirectories(settings);

            var csProjFilePath = $"{settings.ApiDirectory}{Path.DirectorySeparatorChar}{settings.ApiNamespace}.csproj";

            var service = new CsProjService();

            service.AddGenerateDocumentationFile(csProjFilePath);

            service.AddEndpointPostBuildTargetElement(csProjFilePath);

            _launchSettingsBuilder.Build(settings);

            _responseBaseBuilder.Build(settings);

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

        private void CreateSubDirectories(Models.Settings settings)
        {
            _commandService.Start($"mkdir {Constants.Folders.Models}", settings.ApplicationDirectory);

            _commandService.Start($"mkdir {Constants.Folders.Data}", settings.InfrastructureDirectory);

            _commandService.Start($"mkdir {Constants.Folders.Core}", settings.DomainDirectory);

            _commandService.Start($"mkdir {Constants.Folders.Behaviors}", settings.ApplicationDirectory);

            _commandService.Start($"mkdir {Constants.Folders.Extensions}", settings.ApplicationDirectory);

            _commandService.Start($"mkdir {Constants.Folders.Features}", settings.ApplicationDirectory);

            _commandService.Start($"mkdir {Constants.Folders.Interfaces}", settings.ApplicationDirectory);

            _commandService.Start($"mkdir {Constants.Folders.Logs}", settings.ApiDirectory);

        }

        public void RemoveUneededFiles(Models.Settings settings)
        {
            _commandService.Start($"rimraf WeatherForecast.cs", $@"{settings.ApiDirectory}");
            _commandService.Start($@"rimraf Controllers\WeatherForecastController.cs", $@"{settings.ApiDirectory}");
        }

        public void InstallNugetPackages(Models.Settings settings)
        {
            _commandService.Start($"dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 5.0.10", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 5.0.10", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package FluentValidation --version 10.3.3", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package MediatR.Extensions.Microsoft.DependencyInjection  --version 9.0.0", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package Microsoft.EntityFrameworkCore.Tools --version 5.0.10", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package Swashbuckle.AspNetCore --version 6.2.2", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package Swashbuckle.AspNetCore.Swagger --version 6.2.2", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package Newtonsoft.Json", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package Serilog.AspNetCore --version 4.1.0", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package Serilog.Sinks.Seq --version 5.1.1", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package SerilogTimings --version 2.3.0", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package Swashbuckle.AspNetCore.Annotations --version 6.2.2", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package Swashbuckle.AspNetCore.Newtonsoft --version 6.2.2", $@"{settings.ApiDirectory}");
            _commandService.Start($"dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 5.0.9", $@"{settings.ApiDirectory}");
        }
    }
}
*/