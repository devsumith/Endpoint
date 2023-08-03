// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Services;
using System.IO;
using System.Linq;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.Types;
using static SerilogTimings.Operation;
using System.Collections.Generic;
using System.Xml.Linq;
using Endpoint.Core.Syntax.Methods;
using SimpleNLG.Extensions;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax;

namespace Endpoint.Cli.Commands;


[Verb("value-object-create")]
public class ValueObjectCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ValueObjectCreateRequestHandler : IRequestHandler<ValueObjectCreateRequest>
{
    private readonly ILogger<ValueObjectCreateRequestHandler> _logger;
    private readonly IFileProvider _fileProvider;
    private readonly IArtifactGenerator _artifactGenerator;

    public ValueObjectCreateRequestHandler(
        ILogger<ValueObjectCreateRequestHandler> logger,
        IFileProvider fileProvider,
        IArtifactGenerator artifactGenerator
        )
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        _artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(ValueObjectCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ValueObjectCreateRequestHandler));

        var solutionDirectory = Path.GetDirectoryName(_fileProvider.Get("*.sln", request.Directory));

        var valueObjectDefinitionPath = Directory.GetFiles(solutionDirectory, "ValueObject.cs", SearchOption.AllDirectories).FirstOrDefault();

        if (string.IsNullOrEmpty(valueObjectDefinitionPath))
        {
            var templateFileModel = new TemplatedFileModel("ValueObject", "ValueObject", request.Directory, ".cs", new()
            {
                { "Namespace", "System" }
            });

            await _artifactGenerator.CreateAsync(templateFileModel);
        }

        var @class = new ClassModel(request.Name);

        @class.Implements.Add(new TypeModel("ValueObject"));

        @class.UsingDirectives.Add(new("System"));

        var equalityMethod = new MethodModel
        {
            Name = "GetEqualityComponents",
            ReturnType = new TypeModel("IEnumerable<object>"),
            Override = true,
            AccessModifier = AccessModifier.Protected,
            Body = "throw new NotImplementedException();"
        };

        @class.Methods.Add(equalityMethod);

        var classFile = new ObjectFileModel<ClassModel>(
            @class,
            @class.UsingDirectives,
            @class.Name,
            request.Directory,
            ".cs"
            );

        await _artifactGenerator.CreateAsync(classFile);
    }
}
