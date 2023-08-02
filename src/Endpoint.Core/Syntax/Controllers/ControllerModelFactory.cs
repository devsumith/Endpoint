// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Services;
using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax.Controllers;

public class ControllerModelFactory : IControllerModelFactory
{
    private readonly INamingConventionConverter _namingConventionConverter;

    public ControllerModelFactory(INamingConventionConverter namingConventionConverter)
    {
        _namingConventionConverter = namingConventionConverter;
    }

    public ClassModel Create(ClassModel entity)
    {
        throw new NotImplementedException();
    }
}

