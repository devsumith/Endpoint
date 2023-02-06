// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;


namespace Endpoint.Cli.Commands;


[Verb("message-handler-create")]
public class MessageHandlerCreateRequest : IRequest {
    [Option('n',"name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class MessageHandlerCreateRequestHandler : IRequestHandler<MessageHandlerCreateRequest>
{
    private readonly ILogger<MessageHandlerCreateRequestHandler> _logger;

    public MessageHandlerCreateRequestHandler(ILogger<MessageHandlerCreateRequestHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Unit> Handle(MessageHandlerCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(MessageHandlerCreateRequestHandler));

        return new();
    }
}
