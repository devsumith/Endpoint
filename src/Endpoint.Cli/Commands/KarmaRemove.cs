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


[Verb("karma-remove")]
public class KarmaRemoveRequest : IRequest {
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class KarmaRemoveRequestHandler : IRequestHandler<KarmaRemoveRequest>
{
    private readonly ILogger<KarmaRemoveRequestHandler> _logger;
    private readonly IAngularService _angularService;

    public KarmaRemoveRequestHandler(
        ILogger<KarmaRemoveRequestHandler> logger,
        IAngularService angularService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _angularService = angularService ?? throw new ArgumentNullException(nameof(angularService));
    }

    public async Task<Unit> Handle(KarmaRemoveRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(KarmaRemoveRequestHandler));

        _angularService.KarmaRemove(request.Directory);

        return new();
    }
}
