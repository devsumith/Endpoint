// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;
using Endpoint.Core.Syntax.Methods;

namespace Endpoint.Core.Syntax.Controllers;

public class ControllerMethodFactory : IControllerMedthodFactory
{
    private readonly INamingConventionConverter namingConventionConverter;

    public ControllerMethodFactory(INamingConventionConverter namingConventionConverter)
    {
        this.namingConventionConverter = namingConventionConverter;
    }

    public MethodModel Create(ClassModel entity, RouteType routeType)
    {
        throw new NotImplementedException();
    }
}
