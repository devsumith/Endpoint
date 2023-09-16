// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Syntax;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("copyright-add")]
public class CopyrightAddRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class CopyrightAddRequestHandler : IRequestHandler<CopyrightAddRequest>
{
    private readonly ILogger<CopyrightAddRequestHandler> logger;
    private readonly IUtlitityService utlitityService;

    public CopyrightAddRequestHandler(
        ILogger<CopyrightAddRequestHandler> logger,
        IUtlitityService utlitityService)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.utlitityService = utlitityService ?? throw new ArgumentNullException(nameof(utlitityService));
    }

    public async Task Handle(CopyrightAddRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(CopyrightAddRequestHandler));

        utlitityService.CopyrightAdd(request.Directory);
    }
}
