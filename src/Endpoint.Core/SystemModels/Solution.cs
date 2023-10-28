// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Endpoint.Core.SystemModels;

public class Solution
{
    public Solution()
    {
        Microservices = new List<Microservice>();
        BuildingBlocks = new List<BuildingBlock>();
    }

    public string Name { get; set; }

    public List<Microservice> Microservices { get; set; }

    public List<BuildingBlock> BuildingBlocks { get; set; }
}
