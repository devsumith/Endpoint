// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommandLine;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Services;
using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Methods;
using Endpoint.Core.Syntax.Properties;
using Endpoint.Core.Syntax.Types;
using MediatR;
using Microsoft.Extensions.Logging;
using SimpleNLG.Extensions;
using static SerilogTimings.Operation;

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
    private readonly ILogger<ValueObjectCreateRequestHandler> logger;
    private readonly IFileProvider fileProvider;
    private readonly IArtifactGenerator artifactGenerator;

    public ValueObjectCreateRequestHandler(
        ILogger<ValueObjectCreateRequestHandler> logger,
        IFileProvider fileProvider,
        IArtifactGenerator artifactGenerator)
    {
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        this.fileProvider = fileProvider ?? throw new ArgumentNullException(nameof(fileProvider));
        this.artifactGenerator = artifactGenerator ?? throw new ArgumentNullException(nameof(artifactGenerator));
    }

    public async Task Handle(ValueObjectCreateRequest request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handled: {0}", nameof(ValueObjectCreateRequestHandler));

        var solutionDirectory = Path.GetDirectoryName(fileProvider.Get("*.sln", request.Directory));

        var valueObjectDefinitionPath = Directory.GetFiles(solutionDirectory, "ValueObject.cs", SearchOption.AllDirectories).FirstOrDefault();

        if (string.IsNullOrEmpty(valueObjectDefinitionPath))
        {
            var templateFileModel = new TemplatedFileModel("ValueObject", "ValueObject", request.Directory, ".cs", new ()
            {
                { "Namespace", "System" },
            });

            await artifactGenerator.GenerateAsync(templateFileModel);
        }

        var @class = new ClassModel(request.Name);

        @class.Implements.Add(new TypeModel("ValueObject"));

        @class.Usings.Add(new ("System"));

        var equalityMethod = new MethodModel
        {
            Name = "GetEqualityComponents",
            ReturnType = new TypeModel("IEnumerable<object>"),
            Override = true,
            AccessModifier = AccessModifier.Protected,
            Body = new Core.Syntax.Expressions.ExpressionModel("throw new NotImplementedException();"),
        };

        @class.Methods.Add(equalityMethod);

        var classFile = new CodeFileModel<ClassModel>(
            @class,
            @class.Usings,
            @class.Name,
            request.Directory,
            ".cs");

        await artifactGenerator.GenerateAsync(classFile);
    }
}
