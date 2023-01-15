using Endpoint.Core;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts;
using Endpoint.Core.Models.Artifacts.ApiProjectModels;
using Endpoint.Core.Models.Artifacts.Entities;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.Projects;
using Endpoint.Core.Models.Artifacts.Projects.MicroserviceTemplates;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Models.Git;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Attributes;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.Entities.Aggregate;
using Endpoint.Core.Models.Syntax.Entities.Legacy;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Methods.Cqrs;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.RequestHandlers;
using Endpoint.Core.Models.Syntax.RouteHandlers;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Models.WebArtifacts;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies;
using Endpoint.Core.Strategies.Api;
using Endpoint.Core.Strategies.Common;
using Endpoint.Core.Strategies.Files.Create;
using Endpoint.Core.Strategies.Solutions.Crerate;
using Endpoint.Core.Strategies.Solutions.Update;
using Endpoint.Core.Strategies.WorkspaceSettingss.Update;
using MediatR;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddCoreServices(this IServiceCollection services) {
        services.AddSingleton<IAggregateService, AggregateService>();
        services.AddSingleton<ICommandService, CommandService>();
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton<ITemplateProcessor, LiquidTemplateProcessor>();
        services.AddSingleton<INamingConventionConverter, NamingConventionConverter>();
        services.AddSingleton<ISettingsProvider, SettingsProvider>();
        services.AddSingleton<ITenseConverter, TenseConverter>();
        services.AddSingleton<IContext, Context>();
        services.AddSingleton<IFileModelFactory, FileModelFactory>();
        services.AddSingleton<INamespaceProvider, NamespaceProvider>();
        services.AddSingleton<IDomainDrivenDesignFileService, DomainDrivenDesignFileService>();
        services.AddSingleton<IDependencyInjectionService, DependencyInjectionService>();

        services.AddSingleton<IEntityModelFactory, EntityModelFactory>();
        services.AddSingleton<ILegacyAggregateModelFactory, LegacyAggregateModelFactory>();

        services.AddSingleton<IFileNamespaceProvider, FileNamespaceProvider>();
        services.AddSingleton<ISolutionTemplateService, EndpointGenerationStrategy>();
        services.AddSingleton<ISolutionFilesGenerationStrategy, SolutionFilesGenerationStrategy>();
        services.AddSingleton<ISharedKernelProjectFilesGenerationStrategy, SharedKernelProjectFilesGenerationStrategy>();
        services.AddSingleton<IApplicationProjectFilesGenerationStrategy, ApplicationProjectFilesGenerationStrategy>();
        services.AddSingleton<IInfrastructureProjectFilesGenerationStrategy, InfrastructureProjectFilesGenerationStrategy>();
        services.AddSingleton<IApiProjectFilesGenerationStrategy, ApiProjectFilesGenerationStrategy>();
        services.AddSingleton<IProjectService, ProjectService>();
        services.AddSingleton<ISolutionService, SolutionService>();
        services.AddSingleton<IEndpointGenerationStrategyFactory, EndpointGenerationStrategyFactory>();
        services.AddSingleton<IAdditionalResourceGenerationStrategyFactory, AdditionalResourceGenerationStrategyFactory>();
        services.AddSingleton<IFileProvider, FileProvider>();
        services.AddSingleton<ISolutionNamespaceProvider, SolutionNamespaceProvider>();
        services.AddSingleton<ISolutionSettingsFileGenerationStrategyFactory, SolutionSettingsFileGenerationStrategyFactory>();
        services.AddSingleton<ISolutionModelFactory, SolutionModelFactory>();

        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<ClassModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<InterfaceModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<EntityModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, LaunchSettingsFileGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, ConsoleMicroserviceArtifactGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, AggregateArtifactsGenerationStrategy>();

        services.AddSingleton<IArtifactGenerationStrategy, TemplatedFileArtifactGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, SolutionGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, MinimalApiEndpointGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, ProjectGenerationStrategy>();
        services.AddSingleton<IWebApplicationBuilderGenerationStrategy, WebApplicationBuilderGenerationStrategy>();
        services.AddSingleton<IWebApplicationGenerationStrategy, WebApplicationGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, GitGenerationStrategy>();
        services.AddSingleton<IGitGenerationStrategyFactory, GitGenerationStrategyFactory>();
        services.AddSingleton<IArtifactGenerationStrategy, MinimalApiProgramFileGenerationStratey>();
        services.AddSingleton<IWebApplicationBuilderGenerationStrategy, WebApplicationBuilderGenerationStrategy>();
        services.AddSingleton<ISolutionSettingsFileGenerationStrategy, SolutionSettingsFileGenerationStrategy>();
        services.AddSingleton<ISolutionUpdateStrategy, SolutionUpdateStrategy>();
        services.AddSingleton<ISolutionUpdateStrategyFactory, SolutionUpdateStrategyFactory>();
        services.AddSingleton<IMinimalApiService, MinimalApiService>();
        services.AddSingleton<IWorkspaceGenerationStrategyFactory, WorkspaceSettingsGenerationStrategyFactory>();
        services.AddSingleton<IWorkspaceSettingsUpdateStrategyFactory, WorkspaceSettingsUpdateStrategyFactory>();
        services.AddSingleton<IWorkspaceSettingsUpdateStrategy, WorkspaceSettingsUpdateStrategy>();
        services.AddSingleton<IFileModelFactory, FileModelFactory>();
        services.AddSingleton<IFileGenerationStrategyFactory, FileGenerationStrategyFactory>();
        services.AddSingleton<IFileGenerationStrategy, EntityFileGenerationStrategy>();

        services.AddSingleton<IWebArtifactModelsFactory, WebArtifactModelsFactory>();
        services.AddSingleton<IWebGenerationStrategyFactory, WebGenerationStrategyFactory>();
        services.AddSingleton<IWebGenerationStrategy, AngularProjectGenerationStrategy>();

        services.AddSingleton<IArtifactGenerationStrategyFactory, ArtifactGenerationStrategyFactory>();

        services.AddSingleton<ISyntaxGenerationStrategyFactory, SyntaxGenerationStrategyFactory>();
        services.AddSingleton<ISyntaxGenerationStrategy, ClassSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, MethodsSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, MethodSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, InterfaceMethodSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, FieldsSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, ConstructorsSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, ConstructorSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, PropertySyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, PropertiesSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, PropertyAccessorsSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, AccessModifierSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, InterfaceSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RouteHandlerCreateSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RouteHandlerUpdateSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RouteHandlerDeleteSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RouteHandlerGetSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RouteHandlerGetByIdSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, AttributeSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, TypeSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, ParamSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, CreateCommandHandlerMethodGenerationStrategy>();

        services.AddSingleton<ISyntaxGenerationStrategy, RequestHandlerCreateSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RequestHandlerDeleteSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RequestHandlerGetByIdSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RequestHandlerGetSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RequestHandlerSyntaxUpdateGenerationStrategy>();

        services.AddSingleton<IArtifactUpdateStrategyFactory, ArtifactUpdateStrategyFactory>();

        services.AddSingleton<IClipboardService, ClipboardService>();
        
        services.AddSingleton<ISyntaxService>(services =>
        {
            var factory = services.GetRequiredService<IPlantUmlParserStrategyFactory>();
            var fileProvider = services.GetRequiredService<IFileProvider>();
            var fileSystem = services.GetRequiredService<IFileSystem>();

            var args = Environment.GetCommandLineArgs();

            var directoryOptionIndex = Array.IndexOf(args,"-d");

            var directory = directoryOptionIndex != -1 ? args[directoryOptionIndex + 1]  : Environment.CurrentDirectory;

            return new SyntaxService(factory,fileProvider, fileSystem, directory);
        });

        services.AddSingleton<IEntityFileModelFactory, EntityFileModelFactory>();
        services.AddSingleton<IProjectModelFactory, ProjectModelFactory>();

        services.AddSingleton<IRouteHandlerModelFactory, RouteHandlerModelFactory>();
        services.AddMediatR(typeof(Constants));
        services.AddHostedService<ObservableNotificationsProcessor>();


        services.AddSingleton<IPlantUmlParserStrategyFactory, PlantUmlParserStrategyFactory>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlProjectParserStrategy>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlSolutionParserStrategy>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlClassFileParserStrategy>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlInterfaceFileParserStrategy>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlFieldParserStrategy>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlMethodParserStrategy>();
        services.AddSingleton<IPlantUmlParserStrategy, PlantUmlPropertyParserStrategy>();
    }

    public static void AddCoreServices<T>(this IServiceCollection services)
        where T : class
    {
        services.AddSingleton<ITemplateLocator, EmbeddedResourceTemplateLocatorBase<T>>();
        AddCoreServices(services);
    }
}