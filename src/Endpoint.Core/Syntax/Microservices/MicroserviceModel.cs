// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.AggregateModels;
using System;
using System.Collections.Generic;

namespace Endpoint.Core.Syntax.Microservices;

public class MicroserviceModel
{

    List<AggregateModel> AggregatesModels { get; set; }

    public MicroserviceModel(string name)
    {

    }
}

