using System.Collections.Generic;
using Endpoint.Core.Models.Syntax.Entities;

namespace Endpoint.Core.Models.Syntax.Classes;

public class DbContextModel : ClassModel
{
    public List<EntityModel> Entities { get; private set; } = new List<EntityModel>();

    public DbContextModel(string name, List<EntityModel> entities)
        : base(name)
    {
        Name = name;
        Entities = entities;
    }
}
