// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;

[Verb("ref-add")]
public class ReferenceAddRequest : IRequest {
    [Option('s',"service-directory")]
    public string ServiceDirectory { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ReferenceAddRequestHandler : IRequestHandler<ReferenceAddRequest>
{
    private readonly ILogger<ReferenceAddRequestHandler> _logger;
    private readonly ICommandService _commandService;
    private readonly IFileProvider _fileProvider;
    public ReferenceAddRequestHandler(
        ILogger<ReferenceAddRequestHandler> logger,
        ICommandService commandService,
        IFileProvider fileProvider)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
    }

    public async Task Handle(ReferenceAddRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ReferenceAddRequestHandler));

        foreach(var path in Directory.GetFiles(request.ServiceDirectory,"*.csproj", SearchOption.AllDirectories))
        {
            _commandService.Start($"dotnet add {request.Directory} reference {path}", request.Directory);
        }
    }
}
