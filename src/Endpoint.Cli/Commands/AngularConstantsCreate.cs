// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("ng-constants-create")]
public class AngularConstantsCreateRequest : IRequest
{
    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class AngularConstantsCreateRequestHandler : IRequestHandler<AngularConstantsCreateRequest>
{
    private readonly ILogger<AngularConstantsCreateRequestHandler> logger;
    private readonly IArtifactGenerator artifactGenerator;

    public AngularConstantsCreateRequestHandler(
        ILogger<AngularConstantsCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(AngularConstantsCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(AngularConstantsCreateRequestHandler));

        var content = "export const BASE_URL = 'BASE_URL';";

        var model = new ContentFileModel(content, "constants", request.Directory, ".ts");

        await artifactGenerator.GenerateAsync(model);
    }
}
