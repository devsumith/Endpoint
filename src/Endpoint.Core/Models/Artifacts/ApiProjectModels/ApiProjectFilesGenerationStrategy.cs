using Endpoint.Core.Builders;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies.Common;
using Endpoint.Core.ValueObjects;
using System.IO;
using static Endpoint.Core.Constants.ApiFileTemplates;

namespace Endpoint.Core.Models.Artifacts.ApiProjectModels;

public class ApiProjectFilesGenerationStrategy : BaseProjectFilesGenerationStrategy, IApiProjectFilesGenerationStrategy
{
    public ApiProjectFilesGenerationStrategy(
        ICommandService commandService,
        ITemplateProcessor templateProcessor,
        ITemplateLocator templateLocator,
        IFileSystem fileSystem)
        : base(commandService, templateProcessor, templateLocator, fileSystem)
    { }

    public void Build(SettingsModel settings)
    {
        _removeDefaultFiles(settings);

        _commandService.Start("dotnet new tool-manifest", settings.ApiDirectory);

        _commandService.Start("dotnet tool install --version 6.0.7 Swashbuckle.AspNetCore.Cli", settings.ApiDirectory);

        var csProjFilePath = $"{settings.ApiDirectory}{Path.DirectorySeparatorChar}{settings.ApiNamespace}.csproj";

        new BicepFileGenerationStrategy(_fileSystem, _templateLocator).Generate(settings);

        new DeploySetupFileGenerationStrategy(_fileSystem, _templateLocator, _templateProcessor).Generate(settings);

        AddGenerateDocumentationFile(csProjFilePath);

        _addEndpointPostBuildTargetElement(csProjFilePath);

        foreach (var resource in settings.Resources)
        {
            ControllerBuilder.Default(settings, resource.Name, _fileSystem);
        }

        _buildLaunchSettings(settings);

        ProgramBuilder.Build(settings, _templateLocator, _templateProcessor, _fileSystem);

        _buildStartup(settings);
        _buildAppSettings(settings);
        _buildDependencies(settings);
        _installNugetPackages(settings);

    }

    private void _buildAppSettings(SettingsModel settings)
    {
        var template = _templateLocator.Get(AppSettings);

        var tokens = new TokensBuilder()
            .With("RootNamespace", (Token)settings.RootNamespace)
            .With("Directory", (Token)settings.ApiDirectory)
            .With("Namespace", (Token)settings.ApiNamespace)
            .Build();

        var contents = string.Join(Environment.NewLine, _templateProcessor.Process(template, tokens));

        _fileSystem.WriteAllText($@"{settings.ApiDirectory}{Path.DirectorySeparatorChar}appsettings.json", contents);
    }

    private void _buildStartup(SettingsModel settings)
    {
        var template = _templateLocator.Get("Startup");

        var tokens = new TokensBuilder()
            .With("RootNamespace", (Token)settings.RootNamespace)
            .With("Directory", (Token)settings.ApiDirectory)
            .With("Namespace", (Token)settings.ApiNamespace)
            .With("DbContext", (Token)settings.DbContextName)
            .Build();

        var contents = string.Join(Environment.NewLine, _templateProcessor.Process(template, tokens));

        _fileSystem.WriteAllText($@"{settings.ApiDirectory}/Startup.cs", contents);
    }

    private void _buildDependencies(SettingsModel settings)
    {
        var template = _templateLocator.Get(Dependencies);

        var tokens = new TokensBuilder()
            .With("RootNamespace", (Token)settings.RootNamespace)
            .With("Directory", (Token)settings.ApiDirectory)
            .With("Namespace", (Token)settings.ApiNamespace)
            .With("DbContext", (Token)settings.DbContextName)
            .With(nameof(settings.ApplicationNamespace), (Token)settings.ApplicationNamespace)
            .With(nameof(settings.ApiNamespace), (Token)settings.ApiNamespace)
            .With(nameof(settings.DomainNamespace), (Token)settings.DomainNamespace)
            .With(nameof(settings.InfrastructureNamespace), (Token)settings.InfrastructureNamespace)
            .Build();

        var contents = string.Join(Environment.NewLine, _templateProcessor.Process(template, tokens));

        _fileSystem.WriteAllText($@"{settings.ApiDirectory}{Path.DirectorySeparatorChar}Dependencies.cs", contents);
    }

    private void _buildLaunchSettings(SettingsModel settings)
    {
        var template = _templateLocator.Get("LaunchSettings");

        var tokens = new TokensBuilder()
            .With(nameof(settings.RootNamespace), (Token)settings.RootNamespace)
            .With("Directory", (Token)settings.ApiDirectory)
            .With("Namespace", (Token)settings.ApiNamespace)
            .With(nameof(settings.Port), (Token)$"{settings.Port}")
            .With(nameof(settings.SslPort), (Token)$"{settings.SslPort}")
            .With("ProjectName", (Token)settings.ApiNamespace)
            .With("LaunchUrl", (Token)"")
            .Build();

        var contents = string.Join(Environment.NewLine, _templateProcessor.Process(template, tokens));

        _fileSystem.WriteAllText($@"{settings.ApiDirectory}{Path.DirectorySeparatorChar}Properties{Path.DirectorySeparatorChar}launchSettings.json", contents);
    }

    public void _removeDefaultFiles(SettingsModel settings)
    {
        foreach (var path in _fileSystem.GetFiles(settings.ApiDirectory, "*.cs", SearchOption.AllDirectories))
        {
            _fileSystem.Delete(path);
        }
    }

    private void _installNugetPackages(SettingsModel settings)
    {
        _commandService.Start($"dotnet add package Microsoft.EntityFrameworkCore.InMemory --version 6.0.2", $@"{settings.ApiDirectory}");
        _commandService.Start($"dotnet add package MediatR.Extensions.Microsoft.DependencyInjection  --version 10.0.1", $@"{settings.ApiDirectory}");
        _commandService.Start($"dotnet add package Swashbuckle.AspNetCore --version 6.2.3", $@"{settings.ApiDirectory}");
        _commandService.Start($"dotnet add package Swashbuckle.AspNetCore.Swagger --version 6.2.3", $@"{settings.ApiDirectory}");
        _commandService.Start($"dotnet add package Serilog.AspNetCore --version 5.0.0", $@"{settings.ApiDirectory}");
        _commandService.Start($"dotnet add package Serilog.Sinks.Seq --version 5.1.1", $@"{settings.ApiDirectory}");
        _commandService.Start($"dotnet add package SerilogTimings --version 2.3.0", $@"{settings.ApiDirectory}");
        _commandService.Start($"dotnet add package Swashbuckle.AspNetCore.Annotations --version 6.2.3", $@"{settings.ApiDirectory}");
        _commandService.Start($"dotnet add package Swashbuckle.AspNetCore.Newtonsoft --version 6.2.3", $@"{settings.ApiDirectory}");
        _commandService.Start($"dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 6.0.2", $@"{settings.ApiDirectory}");
    }

    public void BuildAdditionalResource(string additionalResource, SettingsModel settings)
    {
        new ClassBuilder($"{((Token)additionalResource).PascalCase}Controller", new Context(), _fileSystem)
            .WithDirectory($"{settings.ApiDirectory}{Path.DirectorySeparatorChar}Controllers")
            .WithUsing("System.Net")
            .WithUsing("System.Threading")
            .WithUsing("System.Threading.Tasks")
            .WithUsing($"{settings.ApplicationNamespace}")
            .WithUsing("MediatR")
            .WithUsing("System")
            .WithUsing("Microsoft.AspNetCore.Mvc")
            .WithUsing("Microsoft.Extensions.Logging")
            .WithUsing("Swashbuckle.AspNetCore.Annotations")
            .WithUsing("System.Net.Mime")
            .WithNamespace($"{settings.ApiNamespace}.Controllers")
            .WithAttribute(new GenericAttributeGenerationStrategy().WithName("ApiController").Build())
            .WithAttribute(new GenericAttributeGenerationStrategy().WithName("Route").WithParam("\"api/[controller]\"").Build())
            .WithAttribute(new GenericAttributeGenerationStrategy().WithName("Produces").WithParam("MediaTypeNames.Application.Json").Build())
            .WithAttribute(new GenericAttributeGenerationStrategy().WithName("Consumes").WithParam("MediaTypeNames.Application.Json").Build())
            .WithDependency("IMediator", "mediator")
            .WithDependency($"ILogger<{((Token)additionalResource).PascalCase}Controller>", "logger")
            .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.GetById).WithResource(additionalResource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Get).WithResource(additionalResource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Create).WithResource(additionalResource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Page).WithResource(additionalResource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Update).WithResource(additionalResource).WithAuthorize(false).Build())
            .WithMethod(new MethodBuilder().WithSettings(settings).WithEndpointType(RouteType.Delete).WithResource(additionalResource).WithAuthorize(false).Build())
            .Build();
    }
}
