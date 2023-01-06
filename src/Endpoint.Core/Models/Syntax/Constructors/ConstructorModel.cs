using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Classes;
using Endpoint.Core.Models.Syntax.Params;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Constructors;

public class ConstructorModel
{
    public ConstructorModel(ClassModel @class, string name)
    {
        Class = @class;
        Name = name;
        Params = new List<ParamModel>();
        BaseParams = new List<string>();
    }

    public ClassModel Class { get; set; }
    public string Name { get; set; }
    public AccessModifier AccessModifier { get; set; }
    public string Body { get; set; }
    public List<string> BaseParams { get; set; }
    public List<ParamModel> Params { get; set; }
}
