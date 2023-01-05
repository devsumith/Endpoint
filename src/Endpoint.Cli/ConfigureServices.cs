using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Microsoft.Extensions.DependencyInjection;

public static class ConfigureServices
{
    public static void AddCliServices(this IServiceCollection services)
    {
        services.AddCoreServices();
        services.AddApplicationServices();
        services.AddInfrastructureServices();

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
            .AddUserSecrets<Program>(optional: true)
            .AddJsonFile("appsettings.json", false)
            .AddEnvironmentVariables()
            .Build();

        services.AddSingleton<IConfiguration>(_ => configuration);
    }
}
