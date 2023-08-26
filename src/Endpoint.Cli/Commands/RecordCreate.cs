using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.Records;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Endpoint.Cli.Commands;


[Verb("record-create")]
public class RecordCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class RecordCreateRequestHandler : IRequestHandler<RecordCreateRequest>
{
    private readonly ILogger<RecordCreateRequestHandler> _logger;
    private readonly IArtifactGenerator _artifactGenerator;

    public RecordCreateRequestHandler(ILogger<RecordCreateRequestHandler> logger, IArtifactGenerator artifactGenerator)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(RecordCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Generating record file. {request.Name}", nameof(RecordCreateRequestHandler));

        var model = new RecordModel(request.Name);

        var file = new ObjectFileModel<RecordModel>(
            model,
            new List<Core.Syntax.UsingDirectiveModel>(),
            model.Name,
            request.Directory,
            ".cs"
            );

        await _artifactGenerator.GenerateAsync(file);
    }
}