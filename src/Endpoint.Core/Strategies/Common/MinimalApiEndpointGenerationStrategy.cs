// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Solutions;
using Endpoint.Core.Artifacts.Git;
using Endpoint.Core.Services;
using System.IO;

namespace Endpoint.Core.Strategies.Common;

public class MinimalApiEndpointGenerationStrategy : ArtifactGenerationStrategyBase<MinimalApiSolutionModel>
{
    private readonly IFileSystem _fileSystem;

    public MinimalApiEndpointGenerationStrategy(IServiceProvider serviceProvider, IFileSystem fileSystem)
        : base(serviceProvider)
    {
        _fileSystem = fileSystem;
    }

    public override void Create(IArtifactGenerator artifactGenerator, MinimalApiSolutionModel model, dynamic context = null)
    {
        var workspaceDirectory = $"{model.Directory}{Path.DirectorySeparatorChar}{model.Name}";

        _fileSystem.CreateDirectory(workspaceDirectory);

        artifactGenerator.CreateFor(new GitModel(model.Name)
        {
            Directory = workspaceDirectory,
        });
    }
}

