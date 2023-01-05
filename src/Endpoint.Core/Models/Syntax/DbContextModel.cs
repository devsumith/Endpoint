using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax;

public class DbContextModel: ClassModel
{
    public List<EntityModel> Entities { get; private set; } = new List<EntityModel>();

    public DbContextModel(string name, List<EntityModel> entities)
        :base(name)
    {
        Name = name;
        Entities = entities;
    }
}
