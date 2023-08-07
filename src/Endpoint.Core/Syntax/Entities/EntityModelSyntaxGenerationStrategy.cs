// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;

namespace Endpoint.Core.Syntax.Entities;

public class EntityModelSyntaxGenerationStrategy : ISyntaxGenerationStrategy<EntityModel>
{
    public EntityModelSyntaxGenerationStrategy(IServiceProvider serviceProvider)

    {

    }

    public int Priority => 0;


    public async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, EntityModel model, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}

