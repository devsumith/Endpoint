// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.AngularProjects;
using Endpoint.Core.Artifacts.Files.Factories;
using Endpoint.Core.Artifacts.Files.Services;
using Endpoint.Core.Artifacts.Folders.Factories;
using Endpoint.Core.Artifacts.Folders.Services;
using Endpoint.Core.Artifacts.Projects.Factories;
using Endpoint.Core.Artifacts.Projects.Services;
using Endpoint.Core.Artifacts.Services;
using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Artifacts.SpecFlow;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes.Factories;
using Endpoint.Core.Syntax.Classes.Services;
using Endpoint.Core.Syntax.Controllers;
using Endpoint.Core.Syntax.Entities;
using Endpoint.Core.Syntax.Entities.Aggregate;
using Endpoint.Core.Syntax.Entities.Legacy;
using Endpoint.Core.Syntax.Methods.Factories;
using Endpoint.Core.Syntax.RouteHandlers;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddCoreServices(this IServiceCollection services)
    {

        services.AddSingleton<IPlaywrightService, PlaywrightService>();
        services.AddSingleton<IMethodFactory, MethodFactory>();
        services.AddSingleton<ISpecFlowService, SpecFlowService>();
        services.AddSingleton<IFolderFactory, FolderFactory>();
        services.AddSingleton<IFolderService, FolderService>();
        services.AddSingleton<IDomainDrivenDesignService, DomainDrivenDesignService>();

        services.AddSingleton<IClassService, ClassService>();
        services.AddSingleton<IUtlitityService, UtlitityService>();
        services.AddSingleton<ISignalRService, SignalRService>();
        services.AddSingleton<IReactService, ReactService>();
        services.AddSingleton<ICoreProjectService, CoreProjectService>();
        services.AddSingleton<ILitService, LitService>();
        services.AddSingleton<IAngularService, AngularService>();
        services.AddSingleton<IInfrastructureProjectService, InfrastructureProjectService>();
        services.AddSingleton<IApiProjectService, ApiProjectService>();
        services.AddSingleton<IAggregateService, AggregateService>();
        services.AddSingleton<ICommandService, CommandService>();
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton<ITemplateProcessor, LiquidTemplateProcessor>();
        services.AddSingleton<INamingConventionConverter, NamingConventionConverter>();
        services.AddSingleton<ISettingsProvider, SettingsProvider>();
        services.AddSingleton<ITenseConverter, TenseConverter>();
        services.AddSingleton<IContext, Context>();
        services.AddSingleton<IFileFactory, FileFactory>();
        services.AddSingleton<INamespaceProvider, NamespaceProvider>();
        services.AddSingleton<IDomainDrivenDesignFileService, DomainDrivenDesignFileService>();
        services.AddSingleton<IDependencyInjectionService, DependencyInjectionService>();
        services.AddSingleton<IEntityFactory, EntityFactory>();
        services.AddSingleton<ILegacyAggregatesFactory, LegacyAggregatesFactory>();
        services.AddSingleton<IFileNamespaceProvider, FileNamespaceProvider>();
        services.AddSingleton<IProjectService, ProjectService>();
        services.AddSingleton<ISolutionService, SolutionService>();
        services.AddSingleton<IFileProvider, FileProvider>();
        services.AddSingleton<ISolutionNamespaceProvider, SolutionNamespaceProvider>();
        services.AddSingleton<ISolutionFactory, SolutionFactory>();
        services.AddSingleton<IControllerFactory, ControllerFactory>();

        services.AddSingleton<IMinimalApiService, MinimalApiService>();
        services.AddSingleton<IFileFactory, FileFactory>();
        services.AddSingleton<IAngularProjectFactory, AngularProjectFactory>();

        services.AddSingleton<IArtifactGenerator, ArtifactGenerator>();
        services.AddSingleton<ISyntaxGenerator, SyntaxGenerator>();

        services.AddSingleton<IClipboardService, ClipboardService>();
        services.AddSingleton<ISyntaxService>(services =>
        {
            var factory = services.GetRequiredService<IPlantUmlParserStrategyFactory>();
            var fileProvider = services.GetRequiredService<IFileProvider>();
            var fileSystem = services.GetRequiredService<IFileSystem>();

            var args = Environment.GetCommandLineArgs();

            var directoryOptionIndex = Array.IndexOf(args, "-d");

            var directory = directoryOptionIndex != -1 ? args[directoryOptionIndex + 1] : Environment.CurrentDirectory;

            return new SyntaxService(factory, fileProvider, fileSystem, directory);
        });

        services.AddSingleton<IEntityFileFactory, EntityFileFactory>();
        services.AddSingleton<IProjectFactory, ProjectFactory>();
        services.AddSingleton<IRouteHandlerFactory, RouteHandlerFactory>();
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(typeof(Constants).Assembly));
        services.AddHostedService<ObservableNotificationsProcessor>();
        services.AddSingleton<IPlantUmlParserStrategyFactory, PlantUmlParserStrategyFactory>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlProjectParserStrategy>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlSolutionParserStrategy>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlClassFileParserStrategy>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlInterfaceFileParserStrategy>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlFieldParserStrategy>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlMethodParserStrategy>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlPropertyParserStrategy>();
        services.AddSingleton<IClassFactory, ClassFactory>();
        services.AddSingleton(typeof(ISyntaxGenerationStrategy<>), typeof(Constants).Assembly);
        services.AddSingleton(typeof(IArtifactGenerationStrategy<>), typeof(Constants).Assembly);
    }

    public static void AddSingleton(this IServiceCollection services, Type @interface, Assembly assembly)
    {
        var implementations = assembly.GetTypes()
            .Where(type =>
                !type.IsAbstract &&
                type.GetInterfaces().Any(interfaceType =>
                    interfaceType.IsGenericType &&
                    interfaceType.GetGenericTypeDefinition() == @interface
                )
            )
            .ToList();

        foreach (var implementation in implementations)
        {
            foreach (var implementedInterface in implementation.GetInterfaces())
            {
                if (implementedInterface.IsGenericType && implementedInterface.GetGenericTypeDefinition() == @interface)
                {
                    services.AddSingleton(implementedInterface, implementation);
                }
            }
        }
    }

    public static void AddCoreServices<T>(this IServiceCollection services)
        where T : class
    {
        services.AddSingleton<ITemplateLocator, EmbeddedResourceTemplateLocatorBase<T>>();
        AddCoreServices(services);
    }
}


















