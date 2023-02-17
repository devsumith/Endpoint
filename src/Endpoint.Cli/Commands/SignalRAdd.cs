// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.WebArtifacts.Services;

namespace Endpoint.Cli.Commands;


[Verb("signalr-add")]
public class SignalRAddRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class SignalRAddRequestHandler : IRequestHandler<SignalRAddRequest>
{
    private readonly ILogger<SignalRAddRequestHandler> _logger;
    private readonly ISignalRService _signalRService;

    public SignalRAddRequestHandler(
        ILogger<SignalRAddRequestHandler> logger,
        ISignalRService signalRService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _signalRService = signalRService ?? throw new ArgumentNullException(nameof(signalRService));
    }

    public async Task Handle(SignalRAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(SignalRAddRequestHandler));

        _signalRService.Add(request.Directory);


    }
}
