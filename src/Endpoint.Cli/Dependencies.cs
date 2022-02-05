using Endpoint.Application;
using Endpoint.Application.Core.Services;
using Endpoint.Application.Services;
using Endpoint.SharedKernal.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Cli
{
    public static class Dependencies
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddMediatR(typeof(Constants),typeof(Marker));
            services.AddSingleton<ICommandService, CommandService>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<ITemplateLocator, TemplateLocator>();
            services.AddSingleton<ITemplateProcessor, LiquidTemplateProcessor>();
            services.AddSingleton<INamingConventionConverter, NamingConventionConverter>();
            services.AddSingleton<ISettingsProvider, SettingsProvider>();
            services.AddSingleton<ITenseConverter, TenseConverter>();
            services.AddSingleton<IContext, Context>();


            services.AddSingleton<ISolutionTemplateService, SolutionTemplateService>();
            services.AddSingleton<ISolutionFileService, SolutionFileService>();
            services.AddSingleton<IDomainFileService, DomainFileService>();
            services.AddSingleton<IApplicationFileService, ApplicationFileService>();
            services.AddSingleton<IInfrastructureFileService, InfrastructureFileService>();
            services.AddSingleton<IApiFileService, ApiFileService>();
        }
    }
}
