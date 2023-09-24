// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax.Units.Factories;

public interface IUnitFactory
{
    Task<SyntaxUnitModel> CreateCommandAsync(ClassModel aggregate, RouteType routeType);
}
