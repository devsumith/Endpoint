// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.Entities;
using System.Collections.Generic;

namespace Endpoint.Core.Services;

public class MinimalApiService : IMinimalApiService
{
    private readonly IArtifactGenerator _artifactGenerator;

    public MinimalApiService(IArtifactGenerator artifactGenerator)
    {
        _artifactGenerator = artifactGenerator;
    }

    public void Create(string name, string dbContextName, string entityName, string directory)
    {
        var entities = new List<EntityModel>()
        {
            new EntityModel(entityName)
        };

        var model = new MinimalApiProgramFileModel(name, directory, name, dbContextName, entities);

        _artifactGenerator.CreateFor(model);
    }
}

