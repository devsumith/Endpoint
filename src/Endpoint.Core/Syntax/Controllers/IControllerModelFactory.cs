// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax.Controllers;

public interface IControllerModelFactory
{
    ClassModel Create(ClassModel entity);
}

