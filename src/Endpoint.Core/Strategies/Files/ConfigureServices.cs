using Endpoint.Core.Strategies.Files.Create;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.Strategies.Files
{
    public static class ServiceCollectionExtensions
    {
        public static void AddFileGenerationApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IFileGenerationStrategyFactory, FileGenerationStrategyFactory>();
            services.AddSingleton<IFileGenerationStrategy, MinimalApiProgramFileGenerationStrategy>();
            services.AddSingleton<IFileGenerationStrategy, TemplatedFileGenerationStrategy>();
            services.AddSingleton<IFileGenerationStrategy, EntityFileGenerationStrategy>();
        }
    }
}
