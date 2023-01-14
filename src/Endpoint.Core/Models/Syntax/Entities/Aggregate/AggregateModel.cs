using Endpoint.Core.Models.Syntax.Classes;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public class AggregateModel
{
    public string MicroserviceName { get; set; }
    public EntityModel Aggregate { get; set; }
    public ClassModel AggregateDto { get; set; }
    public ClassModel AggregateExtensions { get; set; }
    public List<CommandModel> Commands { get; set; }
    public List<QueryModel> Queries { get; set; }

}


public class QueryModel : CqrsBase
{


}

public class CommandModel : CqrsBase
{
    public ClassModel RequestValidator { get; set; }
}

public class CqrsBase
{    
    public ClassModel Request { get; set; }
    public ClassModel Response { get; set; }
    public ClassModel RequestHandler { get; set; }

}
