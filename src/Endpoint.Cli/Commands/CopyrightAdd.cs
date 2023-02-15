// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.Syntax;

namespace Endpoint.Cli.Commands;


[Verb("copyright-add")]
public class CopyrightAddRequest : IRequest {
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class CopyrightAddRequestHandler : IRequestHandler<CopyrightAddRequest>
{
    private readonly ILogger<CopyrightAddRequestHandler> _logger;
    private readonly IUtlitityService _utlitityService;

    public CopyrightAddRequestHandler(
        ILogger<CopyrightAddRequestHandler> logger,
        IUtlitityService utlitityService)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _utlitityService = utlitityService ?? throw new ArgumentNullException(nameof(utlitityService));
    }

    public async Task Handle(CopyrightAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(CopyrightAddRequestHandler));

        _utlitityService.CopyrightAdd(request.Directory);


    }
}
