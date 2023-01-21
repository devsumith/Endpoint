using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;


Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateBootstrapLogger();

Log.Information("Starting Endpoint");

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((services) =>
    {
        services.AddCliServices();
    })
    .Build();

host.Run();