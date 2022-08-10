using Endpoint.Core.Strategies.Solutions.Crerate;
using Endpoint.Core.Strategies.Solutions.Update;
using Endpoint.Core.Strategies.WorkspaceSettingss.Update;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.Strategies.Solutions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSolutionStrategyApplicationServices(this IServiceCollection services)
        {

            services.AddSingleton<ISolutionSettingsFileGenerationStrategy, SolutionSettingsFileGenerationStrategy>();
            services.AddSingleton<ISolutionUpdateStrategy, SolutionUpdateStrategy>();
            services.AddSingleton<ISolutionUpdateStrategyFactory, SolutionUpdateStrategyFactory>();

            services.AddSingleton<IWorkspaceSettingsGenerationStrategy, WorkspaceGenerationStrategy>();
            services.AddSingleton<IWorkspaceGenerationStrategyFactory, WorkspaceSettingsGenerationStrategyFactory>();
            services.AddSingleton<IWorkspaceSettingsUpdateStrategyFactory, WorkspaceSettingsUpdateStrategyFactory>();
            services.AddSingleton<IWorkspaceSettingsUpdateStrategy, WorkspaceSettingsUpdateStrategy>();
            return services;
        }
    }
}
