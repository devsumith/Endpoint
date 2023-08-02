// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Methods;

namespace Endpoint.Core.Syntax.Controllers;

public class ControllerMethodModelFactory : IControllerMedthodModelFactory
{
    private readonly INamingConventionConverter _namingConventionConverter;

    public ControllerMethodModelFactory(INamingConventionConverter namingConventionConverter)
    {
        _namingConventionConverter = namingConventionConverter;
    }

    public MethodModel Create(ClassModel entity, RouteType routeType)
    {
        throw new NotImplementedException();
    }
}

