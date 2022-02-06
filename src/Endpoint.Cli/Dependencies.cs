using Endpoint.Application;
using Endpoint.Application.Core.Extenstions;
using Endpoint.Application.Plugin.ContentManagement;
using Endpoint.Application.Plugin.Identity;
using Endpoint.SharedKernal;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;


namespace Endpoint.Cli
{
    public static class Dependencies
    {
        public static void Configure(IServiceCollection services, string[] plugins)
        {
            services.AddMediatR(typeof(Marker), typeof(Endpoint.SharedKernal.Constants), typeof(Dependencies));
            services.AddMediatR(typeof(Dependencies).Assembly);
            services.AddSingleton<IDomainEvents, DomainEvents>();
            services.AddSharedServices();
            services.AddCoreServices();
            
            if(plugins.Contains("Identity"))
            {
                services.AddMediatR(typeof(IdentityPluginEventHandler));
            }

            if (plugins.Contains("ContentManagement"))
            {
                services.AddMediatR(typeof(ContentManagementPluginEventHandler));
            }
        }

    }


}
