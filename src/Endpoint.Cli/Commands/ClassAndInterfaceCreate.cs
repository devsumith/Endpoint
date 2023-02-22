// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using CommandLine;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Endpoint.Core.Models.Syntax.Classes.Factories;
using Endpoint.Core.Abstractions;
using Endpoint.Core.Models.Artifacts.Files;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Interfaces;
using System.Collections.Generic;
using Endpoint.Core.Models.Syntax.Types;

namespace Endpoint.Cli.Commands;


[Verb("class-and-interface-create")]
public class ClassAndInterfaceCreateRequest : IRequest
{
    [Option('n', "name")]
    public string Name { get; set; }


    [Option('d', Required = false)]
    public string Directory { get; set; } = System.Environment.CurrentDirectory;
}

public class ClassAndInterfaceCreateRequestHandler : IRequestHandler<ClassAndInterfaceCreateRequest>
{
    private readonly ILogger<ClassAndInterfaceCreateRequestHandler> _logger;
    private readonly IClassModelFactory _classModelFactory;
    private readonly IArtifactGenerationStrategyFactory _artifactGenerationStrategyFactory;

    public ClassAndInterfaceCreateRequestHandler(
        ILogger<ClassAndInterfaceCreateRequestHandler> logger,
        IArtifactGenerationStrategyFactory artifactGenerationStrategyFactory,
        IClassModelFactory classModelFactory)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _classModelFactory = classModelFactory ?? throw new ArgumentNullException(nameof(classModelFactory));
        _artifactGenerationStrategyFactory = artifactGenerationStrategyFactory ?? throw new ArgumentNullException(nameof(artifactGenerationStrategyFactory));
    }

    public async Task Handle(ClassAndInterfaceCreateRequest request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handled: {0}", nameof(ClassAndInterfaceCreateRequestHandler));

        foreach (var nameAndMaybeInterfaces in request.Name.Split(','))
        {
            var parts = nameAndMaybeInterfaces.Split(":");
            var name = parts[0];
            var interfaces = new List<TypeModel>();

            if (parts.Length > 1)
            {
                for (var i = 1; i < parts.Length; i++)
                {
                    interfaces.Add(new TypeModel(parts[i]));
                }
            }

            var (classModel, interfaceModel) = _classModelFactory.CreateClassAndInterface(name);

            foreach (var @interface in interfaces)
            {
                interfaceModel.Implements.Add(@interface);
            }

            var classFileModel = new ObjectFileModel<ClassModel>(classModel, classModel.UsingDirectives, classModel.Name, request.Directory, "cs");

            var interfaceFileModel = new ObjectFileModel<InterfaceModel>(interfaceModel, interfaceModel.UsingDirectives, interfaceModel.Name, request.Directory, "cs");

            _artifactGenerationStrategyFactory.CreateFor(classFileModel);

            _artifactGenerationStrategyFactory.CreateFor(interfaceFileModel);
        }
    }
}
