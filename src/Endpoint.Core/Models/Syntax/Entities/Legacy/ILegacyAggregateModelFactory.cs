namespace Endpoint.Core.Models.Syntax.Entities.Legacy;

public interface ILegacyAggregateModelFactory
{
    LegacyAggregateModel Create(string resource, string properties);
}