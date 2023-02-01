// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Entities;

namespace Endpoint.Core.Models.Artifacts.Projects.Services;

public interface IApiProjectService
{
    void ControllerAdd(EntityModel entity, string directory);

}


