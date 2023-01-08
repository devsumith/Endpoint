namespace Endpoint.Core.Models.Syntax.Entities;

public interface IAggregateRootModelFactory
{
    AggregateRootModel Create(string resource, string properties);
}