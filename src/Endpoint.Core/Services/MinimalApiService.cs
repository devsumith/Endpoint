// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Artifacts;
using Endpoint.Core.Artifacts.Files;
using Endpoint.Core.Syntax.Entities;

namespace Endpoint.Core.Services;

public class MinimalApiService : IMinimalApiService
{
    private readonly IArtifactGenerator artifactGenerator;

    public MinimalApiService(IArtifactGenerator artifactGenerator)
    {
        this.artifactGenerator = artifactGenerator;
    }

    public async Task Create(string name, string dbContextName, string entityName, string directory)
    {
        var entities = new List<EntityModel>()
        {
            new EntityModel(entityName),
        };

        var model = new MinimalApiProgramFileModel(name, directory, name, dbContextName, entities);

        await artifactGenerator.GenerateAsync(model);
    }
}
