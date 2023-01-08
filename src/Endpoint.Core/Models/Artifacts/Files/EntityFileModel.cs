using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Entities;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Artifacts.Files;

public class EntityFileModel : ObjectFileModel<EntityModel>
{
    public EntityFileModel(EntityModel @object, string directory)
        :base(@object, new List<UsingDirectiveModel>(),@object.Name,directory,"cs")
    {
        if(@object.UsingDirectives != null)
        {
            foreach(var @using in @object.UsingDirectives)
            {
                Usings.Add(@using);
            }
        }
    }
}
