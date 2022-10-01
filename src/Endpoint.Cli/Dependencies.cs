using Endpoint.Cli.Configuration;
using Endpoint.Cli.Logging;
using Endpoint.Application;
using Endpoint.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Cli
{
    public static class Dependencies
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddSharedServices();
            services.AddCoreServices();
            services.AddApplicationServices();
            services.AddLoggingServices();
            services.AddConfiguration();            
        }
    }
}
