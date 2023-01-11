using Endpoint.Cli;
using Endpoint.Core.Internals;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddCliServices(this IServiceCollection services)        
    {
        
        services.AddLogging(o => o.AddConsole());
        services.AddCoreServices();
        services.AddMediatR(typeof(Program));
        services.AddInfrastructureServices();
        services.AddSingleton(new Observable<INotification>());

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddUserSecrets<Program>(optional: true)
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
            .Build();

        services.AddSingleton<IConfiguration>(_ => configuration);
        services.AddHostedService<CommandLineArgumentsProcessor>();
    }
}

