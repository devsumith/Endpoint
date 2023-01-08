using Endpoint.Core.Models.Artifacts.Files;

namespace Endpoint.Core.Models.Artifacts.Entities;

public interface IEntityFileModelFactory
{
    EntityFileModel Create(string name, string properties, string directory);
}