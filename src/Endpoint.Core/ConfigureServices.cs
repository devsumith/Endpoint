using Endpoint.Core;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.ApiProjectModels;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Artifacts.ProgramFiles;
using Endpoint.Core.Models.Artifacts.Projects;
using Endpoint.Core.Models.Artifacts.Solutions;
using Endpoint.Core.Models.Git;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.WebArtifacts;
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
        services.AddSingleton<ICommandService, CommandService>();
        services.AddSingleton<IFileSystem, FileSystem>();
        services.AddSingleton<ITemplateProcessor, LiquidTemplateProcessor>();
        services.AddSingleton<INamingConventionConverter, NamingConventionConverter>();
        services.AddSingleton<ISettingsProvider, SettingsProvider>();
        services.AddSingleton<ITenseConverter, TenseConverter>();
        services.AddSingleton<IContext, Context>();
        services.AddSingleton<IFileFactory, FileFactory>();
        services.AddSingleton<INamespaceProvider, NamespaceProvider>();

        services.AddSingleton<IFileNamespaceProvider, FileNamespaceProvider>();
        services.AddSingleton<ISolutionTemplateService, EndpointGenerationStrategy>();
        services.AddSingleton<ISolutionFilesGenerationStrategy, SolutionFilesGenerationStrategy>();
        services.AddSingleton<ISharedKernelProjectFilesGenerationStrategy, SharedKernelProjectFilesGenerationStrategy>();
        services.AddSingleton<IApplicationProjectFilesGenerationStrategy, ApplicationProjectFilesGenerationStrategy>();
        services.AddSingleton<IInfrastructureProjectFilesGenerationStrategy, InfrastructureProjectFilesGenerationStrategy>();
        services.AddSingleton<IApiProjectFilesGenerationStrategy, ApiProjectFilesGenerationStrategy>();
        services.AddSingleton<ICsProjectService, CsProjectService>();
        services.AddSingleton<ISolutionService, SolutionService>();
        services.AddSingleton<IEndpointGenerationStrategyFactory, EndpointGenerationStrategyFactory>();
        services.AddSingleton<IAdditionalResourceGenerationStrategyFactory, AdditionalResourceGenerationStrategyFactory>();
        services.AddSingleton<IFileProvider, FileProvider>();
        services.AddSingleton<ISolutionNamespaceProvider, SolutionNamespaceProvider>();
        services.AddSingleton<ISolutionSettingsFileGenerationStrategyFactory, SolutionSettingsFileGenerationStrategyFactory>();
        services.AddSingleton<ISolutionModelFactory, SolutionModelFactory>();


        services.AddSingleton<IArtifactGenerationStrategy, SolutionGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, MinimalApiEndpointGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, ProjectGenerationStrategy>();
        services.AddSingleton<IWebApplicationBuilderGenerationStrategy, WebApplicationBuilderGenerationStrategy>();
        services.AddSingleton<IWebApplicationGenerationStrategy, WebApplicationGenerationStrategy>();
        services.AddSingleton<IArtifactGenerationStrategy, GitGenerationStrategy>();
        services.AddSingleton<IGitGenerationStrategyFactory, GitGenerationStrategyFactory>();
        services.AddSingleton<IMinimalApiProgramFileGenerationStratey, MinimalApiProgramFileGenerationStratey>();
        services.AddSingleton<IWebApplicationBuilderGenerationStrategy, WebApplicationBuilderGenerationStrategy>();
        services.AddSingleton<ISolutionSettingsFileGenerationStrategy, SolutionSettingsFileGenerationStrategy>();
        services.AddSingleton<ISolutionUpdateStrategy, SolutionUpdateStrategy>();
        services.AddSingleton<ISolutionUpdateStrategyFactory, SolutionUpdateStrategyFactory>();

        services.AddSingleton<IWorkspaceSettingsGenerationStrategy, WorkspaceGenerationStrategy>();
        services.AddSingleton<IWorkspaceGenerationStrategyFactory, WorkspaceSettingsGenerationStrategyFactory>();
        services.AddSingleton<IWorkspaceSettingsUpdateStrategyFactory, WorkspaceSettingsUpdateStrategyFactory>();
        services.AddSingleton<IWorkspaceSettingsUpdateStrategy, WorkspaceSettingsUpdateStrategy>();
        services.AddSingleton<IFileFactory, FileFactory>();
        services.AddSingleton<IFileGenerationStrategyFactory, FileGenerationStrategyFactory>();
        services.AddSingleton<IFileGenerationStrategy, MinimalApiProgramFileGenerationStrategy>();
        services.AddSingleton<IFileGenerationStrategy, EntityFileGenerationStrategy>();

        services.AddSingleton<IWebArtifactModelsFactory, WebArtifactModelsFactory>();
        services.AddSingleton<IWebGenerationStrategyFactory, WebGenerationStrategyFactory>();
        services.AddSingleton<IWebGenerationStrategy, AngularProjectGenerationStrategy>();

        services.AddSingleton<IArtifactGenerationStrategyFactory, ArtifactGenerationStrategyFactory>();
        services.AddSingleton<IArtifactGenerationStrategy, TemplatedFileGenerationStrategy>();

        services.AddSingleton<ISyntaxGenerationStrategyFactory, SyntaxGenerationStrategyFactory>();
        services.AddSingleton<ISyntaxGenerationStrategy, ClassSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, MethodSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, FieldsSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, ConstructorSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, PropertySyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, AccessModifierSyntaxGenerationStrategy>();
        services.AddSingleton<ISyntaxGenerationStrategy, InterfaceSyntaxGenerationStrategy>();

        services.AddSingleton<IArtifactUpdateStrategyFactory, ArtifactUpdateStrategyFactory>();

    }
}
