// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core;
using Endpoint.Core.Services;
using Endpoint.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

var configuration = new LoggerConfiguration()
        .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
        .Enrich.FromLogContext()
        .WriteTo.Console();

Log.Logger = configuration.CreateBootstrapLogger();

Log.Information("Starting Endpoint");

var app = CodeGeneratorApplication.CreateBuilder()
    .ConfigureServices(services =>
    {
        services.AddLogging(x => x.AddConsole());
        services.AddSingleton<ITemplateLocator, EmbeddedResourceTemplateLocatorBase<Marker>>();
    })
    .Build();

await app.RunAsync();
