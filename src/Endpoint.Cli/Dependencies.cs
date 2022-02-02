using Endpoint.Application;
using Endpoint.Application.Services;
using Endpoint.Application.Services.FileServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Cli
{
    public static class Dependencies
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddMediatR(typeof(Constants));
            services.AddSingleton<ICommandService, CommandService>();
            services.AddSingleton<IFileSystem, FileSystem>();
            services.AddSingleton<ITemplateLocator, TemplateLocator>();
            services.AddSingleton<ITemplateProcessor, LiquidTemplateProcessor>();
            services.AddSingleton<INamingConventionConverter, NamingConventionConverter>();
            services.AddSingleton<ISettingsProvider, SettingsProvider>();
            services.AddSingleton<ITenseConverter, TenseConverter>();
            services.AddSingleton<IContext, Context>();
            services.AddSingleton<ICsProjService, CsProjService>();


            services.AddSingleton<ISolutionFileService, SolutionFileService>();
            services.AddSingleton<IDomainFileService, DomainFileService>();
            services.AddSingleton<IApplicationFileService, ApplicationFileService>();
        }
    }
}
