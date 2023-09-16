// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax.Entities;

public class EntityModelSyntaxGenerationStrategy : GenericSyntaxGenerationStrategy<EntityModel>
{
    public EntityModelSyntaxGenerationStrategy(IServiceProvider serviceProvider)
    {
    }

    public override async Task<string> GenerateAsync(ISyntaxGenerator syntaxGenerator, EntityModel model)
    {
        throw new NotImplementedException();
    }
}
