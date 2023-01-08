using Endpoint.Core.Models.Syntax.Classes;

namespace Endpoint.Core.Models.Syntax.Entities;

public class EntityModel: ClassModel
{
    public string AggregateRootName { get; private set; }

    public EntityModel(string name)
        :base(name)
    {
        Name = name;
    }
}
