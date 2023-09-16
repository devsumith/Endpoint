// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts.Units;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("message-handler-create")]
public class MessageHandlerCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class MessageHandlerCreateRequestHandler : IRequestHandler<MessageHandlerCreateRequest>
{
    private readonly ILogger<MessageHandlerCreateRequestHandler> logger;
    private readonly IDomainDrivenDesignFileService domainDrivenDesignFileService;

    public MessageHandlerCreateRequestHandler(
        ILogger<MessageHandlerCreateRequestHandler> logger,
        IDomainDrivenDesignFileService domainDrivenDesignFileService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.domainDrivenDesignFileService = domainDrivenDesignFileService ?? throw new ArgumentNullException(nameof(domainDrivenDesignFileService));
    }

    public async Task Handle(MessageHandlerCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(MessageHandlerCreateRequestHandler));

        domainDrivenDesignFileService.MessageHandlerCreate(request.Name, request.Directory);
    }
}
