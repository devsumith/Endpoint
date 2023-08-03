// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Abstractions;

namespace Endpoint.Core.Syntax.Entities;

public class EntityModelSyntaxGenerationStrategy : SyntaxGenerationStrategyBase<EntityModel>
{
    public EntityModelSyntaxGenerationStrategy(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {

    }

    public override async Task<string> CreateAsync(ISyntaxGenerator syntaxGenerator, EntityModel model, dynamic context = null)
    {
        throw new NotImplementedException();
    }
}

