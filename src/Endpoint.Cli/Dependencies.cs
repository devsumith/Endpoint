using Endpoint.Application;
using Endpoint.Application.Core.Extenstions;
using Endpoint.Application.Plugin.ContentManagement;
using Endpoint.Application.Plugin.Identity;
using Endpoint.SharedKernal;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

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
            
            foreach(var plugin in plugins)
            {
                if (plugin == "Identity")
                    services.AddMediatR(typeof(IdentityEndpointPlugin));

                if (plugin == "ContentManagement")
                    services.AddMediatR(typeof(ContentManagementEndpointPlugin));
            }
        }

    }

}
