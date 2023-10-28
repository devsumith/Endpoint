// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Security.Permissions;

namespace Endpoint.Core.SystemModels;

public class Response {

    public string Name { get; set; }
    public List<Property> Properties { get; set; } = new();
}
