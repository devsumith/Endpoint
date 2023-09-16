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

[Verb("karma-remove")]
public class KarmaRemoveRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class KarmaRemoveRequestHandler : IRequestHandler<KarmaRemoveRequest>
{
    private readonly ILogger<KarmaRemoveRequestHandler> logger;
    private readonly IAngularService angularService;

    public KarmaRemoveRequestHandler(
        ILogger<KarmaRemoveRequestHandler> logger,
        IAngularService angularService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task Handle(KarmaRemoveRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(KarmaRemoveRequestHandler));

        await angularService.KarmaRemove(request.Directory);
    }
}
