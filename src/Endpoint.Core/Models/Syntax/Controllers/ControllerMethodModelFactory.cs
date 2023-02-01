// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Services;

namespace Endpoint.Core.Models.Syntax.Controllers;

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

