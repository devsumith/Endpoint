// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;

namespace Endpoint.Core.SystemModels;

public class Controller
{
    public Controller()
    {
    }

    public string Name { get; set; }

    public List<Endpoint> Endpoints { get; set; }
}
