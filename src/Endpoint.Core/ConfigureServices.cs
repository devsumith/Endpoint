using Endpoint.Core.Managers;
using Endpoint.Core.Services;
using Endpoint.Core.Strategies;
using Endpoint.Core.Strategies.Api;
using Endpoint.Core.Strategies.Common;
using Endpoint.Core.Strategies.Common.Git;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSharedServices(this IServiceCollection services)
        {
            services.AddSingleton<ICommandService, CommandService>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<ITemplateLocator, TemplateLocator>();
            services.AddSingleton<ITemplateProcessor, LiquidTemplateProcessor>();
            services.AddSingleton<INamingConventionConverter, NamingConventionConverter>();
            services.AddSingleton<ISettingsProvider, SettingsProvider>();
            services.AddSingleton<ITenseConverter, TenseConverter>();
            services.AddSingleton<IContext, Context>();
        }

        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<ISolutionTemplateService, EndpointGenerationStrategy>();
            services.AddSingleton<ISolutionFilesGenerationStrategy, SolutionFilesGenerationStrategy>();
            services.AddSingleton<ISharedKernelProjectFilesGenerationStrategy, SharedKernelProjectFilesGenerationStrategy>();
            services.AddSingleton<IApplicationProjectFilesGenerationStrategy, ApplicationProjectFilesGenerationStrategy>();
            services.AddSingleton<IInfrastructureProjectFilesGenerationStrategy, InfrastructureProjectFilesGenerationStrategy>();
            services.AddSingleton<IApiProjectFilesGenerationStrategy, ApiProjectFilesGenerationStrategy>();
            services.AddSingleton<ICsProjFileManager, CsProjFileManager>();
            services.AddSingleton<ISolutionFileManager, SolutionFileManager>();
            services.AddSingleton<IEndpointGenerationStrategyFactory, EndpointGenerationStrategyFactory>();
            services.AddSingleton<IAdditionalResourceGenerationStrategyFactory, AdditionalResourceGenerationStrategyFactory>();
            services.AddSingleton<IFileProvider, FileProvider>();
            services.AddSingleton<ISolutionNamespaceProvider, SolutionNamespaceProvider>();
            services.AddSingleton<ISettingsFileGenerationStrategyFactory, SettingsFileGenerationStrategyFactory>();
            services.AddSingleton<IFileGenerationStrategyFactory, FileGenerationStrategyFactory>();
            services.AddSingleton<ISolutionGenerationStrategy, SolutionGenerationStrategy>();
            services.AddSingleton<IProjectGenerationStrategy, ProjectGenerationStrategy>();
            services.AddSingleton<IWebApplicationBuilderGenerationStrategy, WebApplicationBuilderGenerationStrategy>();
            services.AddSingleton<IWebApplicationGenerationStrategy, WebApplicationGenerationStrategy>();

            services.AddGitStrategyApplicationServices();
        }
    }
}
