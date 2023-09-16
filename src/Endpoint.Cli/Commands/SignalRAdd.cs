// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("signalr-add")]
public class SignalRAddRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SignalRAddRequestHandler : IRequestHandler<SignalRAddRequest>
{
    private readonly ILogger<SignalRAddRequestHandler> logger;
    private readonly ISignalRService signalRService;

    public SignalRAddRequestHandler(
        ILogger<SignalRAddRequestHandler> logger,
        ISignalRService signalRService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.signalRService = signalRService ?? throw new ArgumentNullException(nameof(signalRService));
    }

    public async Task Handle(SignalRAddRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(SignalRAddRequestHandler));

        signalRService.Add(request.Directory);
    }
}
