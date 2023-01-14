using Endpoint.Core.Models.Syntax.Classes;
using System;

namespace Endpoint.Core.Models.Syntax.Entities;

public class AggregateModel {

    public EntityModel Aggregate { get; set; }
    public ClassModel AggregateDto { get; set; }
    public ClassModel AggregateExtensions { get; set; }

}


public class QueryModel: CqrsBase
{
    

}

public class CommandModel: CqrsBase
{
    public ClassModel RequestValidator { get; set; }
}

public class CqrsBase
{
    public ClassModel Request { get; set; }
    public ClassModel Response { get; set; }
    public ClassModel RequestHandler { get; set; }

}
