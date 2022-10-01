using Endpoint.Cli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace Endpoint.Cli.Configuration
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddConfiguration(this IServiceCollection services)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddUserSecrets<Program>(optional: true)
                .AddJsonFile("appsettings.json", false)   
                .AddEnvironmentVariables()
                .Build();

            services.AddSingleton<IConfiguration>(_ => configuration);

            return services;
        }
    }
}
