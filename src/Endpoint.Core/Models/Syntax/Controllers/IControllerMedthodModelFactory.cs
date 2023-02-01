// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Methods;

namespace Endpoint.Core.Models.Syntax.Controllers;

public interface IControllerMedthodModelFactory
{
    
    MethodModel Create(ClassModel entity, RouteType routeType);
}

