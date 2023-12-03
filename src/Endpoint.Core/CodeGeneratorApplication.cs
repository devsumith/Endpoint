// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Threading;
using CommandLine;
using Endpoint.Core.Extensions;
using Endpoint.Core.Internals;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Core;

public class CodeGeneratorApplication
{
    private readonly IMediator mediator;
    private readonly ILogger<CodeGeneratorApplication> logger;

    public CodeGeneratorApplication(IMediator mediator, ILogger<CodeGeneratorApplication> logger, Observable<INotification> notificationObservable)
    {
        this.mediator = mediator;
        this.logger = logger;

        _ = notificationObservable.Subscribe(async x =>
        {
            await mediator.Publish(x);
        });
    }

    public static CodeGeneratorApplicationBuilder CreateBuilder()
    {
        var builder = new CodeGeneratorApplicationBuilder();
        return builder;
    }

    public async Task RunAsync(CancellationToken token = default)
    {
        await mediator.Send(Environment.GetCommandLineArgs().ParseArguments());
    }
}