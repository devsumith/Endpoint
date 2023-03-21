// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Entities.Aggregate;
using System;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Microservices;

public class MicroserviceModel
{

    List<AggregatesModel> AggregatesModels { get; set; }

    public MicroserviceModel(string name)
    {

    }
}

