using Endpoint.Core.Models.Syntax.Classes;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Entities.Aggregate;

public class CqrsBase
{
    public CqrsBase()
    {
        UsingDirectives = new List<UsingDirectiveModel>();
    }
    public string Name { get; set; }
    public ClassModel Request { get; set; }
    public ClassModel Response { get; set; }
    public ClassModel RequestHandler { get; set; }
    public List<UsingDirectiveModel> UsingDirectives { get; set; }
}
