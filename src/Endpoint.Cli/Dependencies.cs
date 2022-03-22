using Allagi.Endpoint.Cli.Logging;
using Endpoint.Application;
using Endpoint.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Endpoint.Cli
{
    public static class Dependencies
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddMediatR(typeof(CoreConstants), typeof(ApplicationConstants));
            services.AddSharedServices();
            services.AddCoreServices();
            services.AddSingleton(CreateLoggerFactory().CreateLogger("endpoint"));

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddUserSecrets<Program>(optional: true)                
                .AddJsonFile("appsettings.json", false)
                .Build();

            services.AddSingleton<IConfiguration>(_ => configuration);
        }

        private static ILoggerFactory CreateLoggerFactory()
        {
            return LoggerFactory.Create(builder =>
            {
                builder.AddProvider(new EndpointLoggerProvider(new EndpointLoggerOptions(true, ConsoleColor.Red, ConsoleColor.DarkYellow, Console.Out)));
            });
        }
    }
}
