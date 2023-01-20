using Endpoint.Core.Models.Syntax.Entities;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Classes.Factories;

public interface IClassModelFactory
{
    ClassModel CreateEntity(string name, string properties);
    ClassModel CreateController(EntityModel model, string directory);
    ClassModel CreateDbContext(string name, List<EntityModel> entities, string directory);
}
