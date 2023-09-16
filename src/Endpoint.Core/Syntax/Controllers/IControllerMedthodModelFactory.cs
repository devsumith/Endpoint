// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Methods;

namespace Endpoint.Core.Syntax.Controllers;

public interface IControllerMedthodFactory
{
    MethodModel Create(ClassModel entity, RouteType routeType);
}
