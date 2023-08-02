// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System;
using System.Linq;
using System.Threading.Tasks;

await RunAsync();
async Task RunAsync()
{
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

    var _configuration = host.Services.GetRequiredService<IConfiguration>();

    var _mediator = host.Services.GetRequiredService<IMediator>();

    var args = Environment.GetCommandLineArgs().Skip(1).ToArray();

    if (args.Length == 0 || args[0].StartsWith('-'))
    {
        args = new string[1] { _configuration[Constants.EnvironmentVariables.DefaultCommand] }.Concat(args).ToArray();
    }

    try
    {
        var verbs = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(type => type.GetCustomAttributes(typeof(VerbAttribute), true).Length > 0)
            .ToArray();

        var parsedResult = _createParser().ParseArguments(args, verbs);

        if (parsedResult.Errors.SingleOrDefault() is HelpRequestedError || parsedResult.Errors.SingleOrDefault() is HelpRequestedError)
        {
            Environment.Exit(0);
        }

        if (parsedResult.Errors.SingleOrDefault() is BadVerbSelectedError error)
        {
            Log.Error("{tag}:{token}", error.Tag, error.Token);

            throw new Exception($"{error.Tag}:{error.Token}");
        }

        await parsedResult.WithParsedAsync(request => _mediator.Send(request));


    }
    catch (Exception ex)
    {
        Log.Error(ex.Message);
    }
    finally
    {
        Environment.Exit(0);
    }
}

Parser _createParser() => new Parser(with =>
{
    with.CaseSensitive = false;
    with.HelpWriter = Console.Out;
    with.IgnoreUnknownArguments = true;
});