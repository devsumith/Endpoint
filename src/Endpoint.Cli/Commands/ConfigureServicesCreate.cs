// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("configure-services-create")]
public class ConfigureServicesCreateRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ConfigureServicesCreateRequestHandler : IRequestHandler<ConfigureServicesCreateRequest>
{
    private readonly ILogger<ConfigureServicesCreateRequestHandler> _logger;
    private readonly IDependencyInjectionService _dependencyInjectionService;
    private readonly IFileProvider _fileProvider;

    public ConfigureServicesCreateRequestHandler(
        ILogger<ConfigureServicesCreateRequestHandler> logger,
        IDependencyInjectionService dependencyInjectionService,
        IFileProvider fileProvider)
    {
        _dependencyInjectionService = dependencyInjectionService ?? throw new ArgumentNullException(nameof(dependencyInjectionService));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(ConfigureServicesCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ConfigureServicesCreateRequestHandler));

        var csProjectPath = _fileProvider.Get("*.csproj", request.Directory);

        var directory = Path.GetDirectoryName(csProjectPath);

        var parts = Path.GetFileNameWithoutExtension(csProjectPath).Split('.');

        var layer = parts.Count() > 0 ? parts.Last() : string.Empty;

        _dependencyInjectionService.AddConfigureServices(layer, request.Directory);


    }
}
