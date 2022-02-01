using Endpoint.Application;
using Endpoint.Application.Builders;
using Endpoint.Application.Services;
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


            services.AddSingleton<ISolutionBuilder, SolutionBuilder>();

            services.AddSingleton<IApiFileBuilder, ApiFileBuilder>();
        }
    }
}
