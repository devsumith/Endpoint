using Endpoint.Application.Core.Services;
using Endpoint.Application.Services;
using Endpoint.SharedKernal.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Application.Core.Extenstions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCoreServices(this IServiceCollection services)
        {
            services.AddSingleton<ISolutionTemplateService, SolutionTemplateService>();
            services.AddSingleton<ISolutionFileService, SolutionFileService>();
            services.AddSingleton<IDomainFileService, DomainFileService>();
            services.AddSingleton<IApplicationFileService, ApplicationFileService>();
            services.AddSingleton<IInfrastructureFileService, InfrastructureFileService>();
            services.AddSingleton<IApiFileService, ApiFileService>();
        }
    }
}
