// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Endpoint.Cli.Commands;

[Verb("value-type-create")]
public class ValueTypeCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }

    [Option('v', "value")]
    public string Value { get; set; }

    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ValueTypeCreateRequestHandler : IRequestHandler<ValueTypeCreateRequest>
{
    private readonly ILogger<ValueTypeCreateRequestHandler> logger;
    private readonly IArtifactGenerator artifactGenerator;
    private readonly INamingConventionConverter namingConventionConverter;

    public ValueTypeCreateRequestHandler(
        ILogger<ValueTypeCreateRequestHandler> logger,
        IArtifactGenerator artifactGenerator,
        INamingConventionConverter namingConventionConverter)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        this.namingConventionConverter = namingConventionConverter ?? throw new ArgumentNullException(nameof(namingConventionConverter));
    }

    public async Task Handle(ValueTypeCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(ValueTypeCreateRequestHandler));

        var tokens = new TokensBuilder()
            .With(nameof(request.Name), request.Name)
            .With(nameof(request.Value), request.Value)
            .With("namespace", "System")
            .Build();

        var model = new TemplatedFileModel("ValueType", $"{namingConventionConverter.Convert(NamingConvention.PascalCase, request.Name)}Type", request.Directory, ".cs", tokens);

        await artifactGenerator.GenerateAsync(model);
    }
}
