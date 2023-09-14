// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Classes.Factories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using static Endpoint.Core.Constants.FileExtensions;

namespace Endpoint.Cli.Commands;


[Verb("response-create")]
public class ResponseCreateRequest : IRequest {
    [Option('r',"response-type")]
    public string ResponseType { get; set; }


    [Option('e', "entity-name")]
    public string EntityName { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ResponseCreateRequestHandler : IRequestHandler<ResponseCreateRequest>
{
    private readonly ILogger<ResponseCreateRequestHandler> _logger;
    private readonly IArtifactGenerator _artifactGenerator;
    private readonly IClassFactory _classFactory;

    public ResponseCreateRequestHandler(IArtifactGenerator artifactGenerator, IClassFactory classFactory, ILogger<ResponseCreateRequestHandler> logger)
    {
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
        _classFactory = classFactory ?? throw new ArgumentNullException(nameof(classFactory));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(ResponseCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Creating Response. {name}", request.ResponseType);;

        var model = await _classFactory.CreateResponseAsync(request.ResponseType.ToRequestType(), request.EntityName);

        await _artifactGenerator.GenerateAsync(new ObjectFileModel<ClassModel>(model, model.Name, request.Directory, CSharpFile));

    }
}
