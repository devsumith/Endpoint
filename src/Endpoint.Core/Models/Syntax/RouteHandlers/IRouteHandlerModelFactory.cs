// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Entities.Legacy;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.RouteHandlers;

public interface IRouteHandlerModelFactory
{
    List<RouteHandlerModel> Create(string dbContextName, LegacyAggregateModel aggregateRoot);
}
