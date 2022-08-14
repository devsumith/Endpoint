using Endpoint.Core.Strategies.Api.FileGeneration;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core.Strategies.Api
{
    public static class ServiceCollectionExtensions
    {
        public static void AddApiFileGenerationServices(this IServiceCollection services)
        {
            services.AddSingleton<IMinimalApiProgramGenerationStratey, MinimalApiProgramGenerationStratey>();
            services.AddSingleton<IWebApplicationBuilderGenerationStrategy, WebApplicationBuilderGenerationStrategy>();
            services.AddSingleton<IWebApplicationGenerationStrategy, WebApplicationGenerationStrategy>();

        }
    }
}
