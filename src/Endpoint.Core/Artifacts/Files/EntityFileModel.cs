// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax;
using Endpoint.Core.Syntax.Entities;
using System.Collections.Generic;

namespace Endpoint.Core.Artifacts.Files;

public class EntityFileModel : ObjectFileModel<EntityModel>
{
    public EntityFileModel(EntityModel @object, string directory)
        : base(@object, new List<UsingDirectiveModel>(), @object.Name, directory, "cs")
    {
        if (@object.UsingDirectives != null)
        {
            foreach (var @using in @object.UsingDirectives)
            {
                Usings.Add(@using);
            }
        }
    }
}

