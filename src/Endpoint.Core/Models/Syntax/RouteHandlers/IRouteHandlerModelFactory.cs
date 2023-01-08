using Endpoint.Core.Models.Syntax.Entities;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.RouteHandlers;

public interface IRouteHandlerModelFactory
{
    List<RouteHandlerModel> Create(string dbContextName, AggregateRootModel aggregateRoot);
}