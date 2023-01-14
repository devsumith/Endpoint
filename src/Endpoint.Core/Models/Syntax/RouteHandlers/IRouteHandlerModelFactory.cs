using Endpoint.Core.Models.Syntax.Entities.Legacy;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.RouteHandlers;

public interface IRouteHandlerModelFactory
{
    List<RouteHandlerModel> Create(string dbContextName, LegacyAggregateModel aggregateRoot);
}