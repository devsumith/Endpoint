// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Linq;
using CommandLine;
using Endpoint.Core.Internals;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Endpoint.Core;

public class CodeGeneratorApplication
{
    private readonly IServiceProvider serviceProvider;

    public CodeGeneratorApplication(IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(serviceProvider, nameof(serviceProvider));

        this.serviceProvider = serviceProvider;
    }

    public static CodeGeneratorApplicationBuilder CreateBuilder()
    {
        var builder = new CodeGeneratorApplicationBuilder();
        return builder;
    }

    public async Task RunAsync()
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var notificationObservable = serviceProvider.GetRequiredService<Observable<INotification>>();

        notificationObservable.Subscribe(async x =>
        {
            await mediator.Publish(x);
        });

        try
        {
            await mediator.Send(ParseCommandLineArgs());
        }
        finally
        {
            Environment.Exit(0);
        }
    }

    object ParseCommandLineArgs()
    {
        var parser = new Parser(with =>
        {
            with.CaseSensitive = false;
            with.HelpWriter = Console.Out;
            with.IgnoreUnknownArguments = true;
        });

        var args = Environment.GetCommandLineArgs().Skip(1).ToArray();

        if (args.Length == 0 || args[0].StartsWith('-'))
        {
            args = new string[1] { "default" }.Concat(args).ToArray();
        }

        var verbs = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(type => type.GetCustomAttributes(typeof(VerbAttribute), true).Length > 0)
            .ToArray();

        var parsedResult = parser.ParseArguments(args, verbs);

        if (parsedResult.Errors.SingleOrDefault() is HelpRequestedError || parsedResult.Errors.SingleOrDefault() is HelpRequestedError)
        {
            Environment.Exit(0);
        }

        if (parsedResult.Errors.SingleOrDefault() is BadVerbSelectedError error)
        {
            throw new Exception($"{error.Tag}:{error.Token}");
        }

        return parsedResult.Value;
    }
}


