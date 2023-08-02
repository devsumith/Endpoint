// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Entities;

namespace Endpoint.Core.Options;

public class UpdateCleanArchitectureMicroserviceOptions
{
    public string Directory { get; set; }
    public EntityModel Entity { get; set; }
}

