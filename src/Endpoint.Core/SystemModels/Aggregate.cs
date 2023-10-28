// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Endpoint.Core.SystemModels;

public class Aggregate {

    public Aggregate()
    {
        Properties = new List<Property>();
    }

    public string Name { get; set; }
    public List<Property> Properties { get; set; }
}
