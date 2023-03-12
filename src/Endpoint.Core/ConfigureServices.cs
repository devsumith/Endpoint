// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts;
using Endpoint.Core.Models.Artifacts.Files.Factories;
using Endpoint.Core.Models.Artifacts.Files.Services;
using Endpoint.Core.Models.Artifacts.Files.Strategies;
using Endpoint.Core.Models.Artifacts.Folders.Factories;
using Endpoint.Core.Models.Artifacts.Folders.Services;
using Endpoint.Core.Models.Artifacts.Folders.Strategies;
using Endpoint.Core.Models.Artifacts.Git;
using Endpoint.Core.Models.Artifacts.Projects;
using Endpoint.Core.Models.Artifacts.Projects.Factories;
using Endpoint.Core.Models.Artifacts.Projects.Services;
using Endpoint.Core.Models.Artifacts.Projects.Strategies;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Models.Artifacts.SpecFlow;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Attributes.Strategies;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Classes.Factories;
using Endpoint.Core.Models.Syntax.Classes.Services;
using Endpoint.Core.Models.Syntax.Classes.Strategies;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Controllers;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.Entities.Aggregate;
using Endpoint.Core.Models.Syntax.Entities.Legacy;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Methods.Factories;
using Endpoint.Core.Models.Syntax.Methods.RequestHandlerMethodBodies;
using Endpoint.Core.Models.Syntax.Params;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.RequestHandlers;
using Endpoint.Core.Models.Syntax.RouteHandlers;
using Endpoint.Core.Models.Syntax.SpecFlow;
using Endpoint.Core.Models.Syntax.SpecFlow.strategies;
using Endpoint.Core.Models.Syntax.SpecFlow.Strategies;
using Endpoint.Core.Models.Syntax.Types;
using Endpoint.Core.Models.Syntax.TypeScript;
using Endpoint.Core.Models.Syntax.TypeScript.Strategies;
using Endpoint.Core.Models.WebArtifacts.Factories;
using Endpoint.Core.Models.WebArtifacts.Services;
using Endpoint.Core.Models.WebArtifacts.Strategies;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies;
using Endpoint.Core.Strategies.Api;
using Endpoint.Core.Strategies.Common;
using Endpoint.Core.Strategies.Files.Create;
using Endpoint.Core.Strategies.Solutions.Crerate;
using Endpoint.Core.Strategies.Solutions.Update;
using Endpoint.Core.Strategies.WorkspaceSettingss.Update;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddCoreServices(this IServiceCollection services)
    {
        services.AddSingleton<IMethodModelFactory, MethodModelFactory>();
        //services.AddSingleton<ISyntaxGenerationStrategy, RequestValidatorSyntaxGenerationStrategy>();
        //services.AddSingleton<ISyntaxGenerationStrategy, RequestSyntaxGenerationStrategy>();
        //services.AddSingleton<ISyntaxGenerationStrategy, ResponseSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RuleForSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, TypeScriptTypeSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, ImportSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, FunctionSyntaxGenerationStrategy>();
        services.AddSingleton<ISpecFlowService, SpecFlowService>();
        services.AddSingleton<ISyntaxGenerationStrategy, SpecFlowFeatureSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, SpecFlowHookSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, SpecFlowStepsSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, SpecFlowScenarioSyntaxGenerationStrategy>();
        services.AddSingleton<IFolderFactory, FolderFactory>();
        services.AddSingleton<IFolderService, FolderService>();
        services.AddSingleton<IDomainDrivenDesignService, DomainDrivenDesignService>();
        services.AddSingleton<ISyntaxGenerationStrategy, TestReferenceSyntaxGenerationStrategy>();
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
        services.AddSingleton<IControllerModelFactory, ControllerModelFactory>();

        services.AddSingleton<IArtifactGenerationStrategy, FolderArtifactGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<ClassModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<InterfaceModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<EntityModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<QueryModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<CommandModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<SpecFlowFeatureModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<SpecFlowHookModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<SpecFlowStepsModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<TypeScriptTypeModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, ObjectFileArtifactGenerationStrategyBase<FunctionModel>>();
        services.AddSingleton<IArtifactGenerationStrategy, LaunchSettingsFileGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, ConsoleMicroserviceArtifactGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, AggregateArtifactsGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, AngularWorkspaceArtifactGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, LitWorkspaceArtifactGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, AddAngularTranslateGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, InfrastructureProjectEnsureArtifactGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, CoreProjectEnsureArtifactGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, ApiProjectEnsureArtifactGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, CopyrightAddArtifactGenerationStrategy>();

        services.AddSingleton<IArtifactGenerationStrategy, TemplatedFileArtifactGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, SolutionGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, MinimalApiEndpointGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, ProjectGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, ContentFileArtifactGenerationStrategy>();
        services.AddSingleton<IWebApplicationBuilderGenerationStrategy, WebApplicationBuilderGenerationStrategy>();
        services.AddSingleton<IWebApplicationGenerationStrategy, WebApplicationGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, GitGenerationStrategy>();
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
        services.AddSingleton<ISyntaxGenerationStrategy, ProducesResponseTypeAttributeSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, SwaggerOperationAttributeSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, TypeSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, ParamSyntaxGenerationStrategy>();

        services.AddSingleton<ISyntaxGenerationStrategy, CreateCommandHandlerMethodGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, UpdateCommandHandlerMethodGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, DeleteCommandHandlerMethodGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, GetQueryHandlerMethodGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, GetByIdQueryHandlerMethodGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, PageQueryHandlerMethodGenerationStrategy>();

        services.AddSingleton<ISyntaxGenerationStrategy, RequestHandlerCreateSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RequestHandlerDeleteSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RequestHandlerGetByIdSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RequestHandlerGetSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, RequestHandlerSyntaxUpdateGenerationStrategy>();

        services.AddSingleton<ISyntaxGenerationStrategy, QueryModelSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, CommandModelSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, ControllerSyntaxGenerationStrategy>();

        services.AddSingleton<IArtifactUpdateStrategyFactory, ArtifactUpdateStrategyFactory>();

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

        services.AddSingleton<IEntityFileModelFactory, EntityFileModelFactory>();
        services.AddSingleton<IProjectModelFactory, ProjectModelFactory>();

        services.AddSingleton<IRouteHandlerModelFactory, RouteHandlerModelFactory>();
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

        services.AddSingleton<IClassModelFactory, ClassModelFactory>();
    }

    public static void AddCoreServices<T>(this IServiceCollection services)
        where T : class
    {
        services.AddSingleton<ITemplateLocator, EmbeddedResourceTemplateLocatorBase<T>>();
        AddCoreServices(services);
    }
}

















