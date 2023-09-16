// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Endpoint.Core.Syntax.Units;

namespace Endpoint.Core.Syntax.Microservices;

public class MicroserviceModel
{
    public MicroserviceModel(string name)
    {
    }

    private List<AggregateModel> AggregatesModels { get; set; }
}
