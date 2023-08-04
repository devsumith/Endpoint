// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

namespace Endpoint.Core.Syntax.Methods.Factories;

public interface IMethodFactory
{
    MethodModel CreateControllerMethod(string name, string controller, RouteType routeType, string directory);

}


