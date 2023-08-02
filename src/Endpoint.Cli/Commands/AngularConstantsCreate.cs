// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("ng-constants-create")]
public class AngularConstantsCreateRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularConstantsCreateRequestHandler : IRequestHandler<AngularConstantsCreateRequest>
{
    private readonly ILogger<AngularConstantsCreateRequestHandler> _logger;
    private readonly IArtifactGenerator _artifactGenerator;

    public AngularConstantsCreateRequestHandler(
        ILogger<AngularConstantsCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(AngularConstantsCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(AngularConstantsCreateRequestHandler));

        var content = "export const BASE_URL = 'BASE_URL';";

        var model = new ContentFileModel(content, "constants", request.Directory, "ts");

        _artifactGenerator.CreateFor(model);
    }
}
