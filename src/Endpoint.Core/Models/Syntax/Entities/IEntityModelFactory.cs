namespace Endpoint.Core.Models.Syntax.Entities;

public interface IEntityModelFactory
{
    EntityModel Create(string name, string properties);
}