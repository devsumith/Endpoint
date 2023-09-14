// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax.Methods.Factories;

public interface IMethodFactory
{
    MethodModel CreateControllerMethod(string name, string controller, RouteType routeType, string directory);

    Task<MethodModel> ToDtoCreateAsync(ClassModel aggregate);

    Task<MethodModel> ToDtosAsyncCreateAsync(ClassModel aggregate);

    Task<MethodModel> CreateWorkerExecuteAsync();

}


