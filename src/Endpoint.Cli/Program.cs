using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

Log.Information("Starting Endpoint");

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((services) =>
    {
        services.AddCliServices();
    })
    .Build();

host.Run();