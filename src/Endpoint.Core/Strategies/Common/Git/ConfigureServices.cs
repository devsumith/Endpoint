using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.Strategies.Common.Git
{
    public static class ServiceCollectionExtensions 
    {
        public static IServiceCollection AddGitStrategyApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IGitGenerationStrategy, GitGenerationStrategy>();
            services.AddSingleton<IGitGenerationStrategyFactory, GitGenerationStrategyFactory>();
            return services;
        }
    }
}
