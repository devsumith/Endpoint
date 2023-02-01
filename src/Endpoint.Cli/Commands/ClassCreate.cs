// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace Endpoint.Cli.Commands;


[Verb("class-create")]
public class ClassCreateRequest : IRequest<Unit>
{

    [Option('n')]
    public string Name { get; set; }

    [Option('p')]
    public string Properties { get; set; }

    [Option('d')]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ClassCreateRequestHandler : IRequestHandler<ClassCreateRequest, Unit>
{
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;

    public ClassCreateRequestHandler(IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory)
    {
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory;
    }

    public Task<Unit> Handle(ClassCreateRequest request, CancellationToken cancellationToken)
    {
        var @class = new ClassModel(request.Name);

        @class.UsingDirectives.Add(new UsingDirectiveModel() {  Name = "System"  });

        if(!string.IsNullOrEmpty(request.Properties))
            foreach(var property in request.Properties.Split(','))
            {
                var parts = property.Split(':');
                var name = parts[0];
                var type = parts[1];

                @class.Properties.Add(new PropertyModel(@class, AccessModifier.Public, new TypeModel() { Name = type }, name, new List<PropertyAccessorModel>()));
            }

        var classFile = new ObjectFileModel<ClassModel>(
            @class,
            @class.UsingDirectives,
            @class.Name,
            request.Directory,
            "cs"
            );

        _artifactGenerationStrategyFactory.CreateFor(classFile);

        return Task.FromResult(new Unit());
    }
}

