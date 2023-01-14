using Endpoint.Core.Models.Syntax.Entities.Aggregate;
using System;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Microservices;

public class MicroserviceModel { 

    List<AggregateModel> AggregateModels { get; set; }

    public MicroserviceModel(string name)
    {
        
    }
}
