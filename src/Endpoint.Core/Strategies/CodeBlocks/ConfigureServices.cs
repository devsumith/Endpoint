using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.Strategies.CodeBlocks
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCodeBlockGenerationServices(this IServiceCollection services)
        {
            services.AddSingleton<ICodeBlockGenerationStrategyFactory, CodeBlockGenerationStrategyFactory>();
            services.AddSingleton<ICodeBlockGenerationStrategy, EntityCodeBlockGenerationStrategy>();
        }
    }
}
