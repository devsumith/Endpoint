namespace Endpoint.Core.Models.Syntax.Classes;

public interface IClassModelFactory
{
    ClassModel CreateEntity(string name, string properties);
}
