using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Endpoint.Cli.Logging
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLoggingServices(this IServiceCollection services)
        {
            services.AddSingleton(CreateLoggerFactory().CreateLogger("endpoint"));

            return services;
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
