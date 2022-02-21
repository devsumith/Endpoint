using Allagi.Endpoint.Cli.Logging;
using Endpoint.Application;
using Endpoint.Core;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Endpoint.Cli
{
    public static class Dependencies
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddMediatR(typeof(CoreConstants), typeof(ApplicationConstants));
            services.AddSharedServices();
            services.AddCoreServices();
            services.AddSingleton(CreateLoggerFactory().CreateLogger("bicep"));
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
