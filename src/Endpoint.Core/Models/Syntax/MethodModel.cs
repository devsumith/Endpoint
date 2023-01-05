using Endpoint.Core.Enums;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax;

public class MethodModel
{
    public MethodModel()
    {
        Params = new List<ParamModel>();
        Attributes = new List<AttributeModel>();
    }

    public List<ParamModel> Params { get; set; }
    public List<AttributeModel> Attributes { get; set; }
    public AccessModifier AccessModifier { get; set; }
    public string Name { get; set; }
    public bool Override { get; set; }
    public TypeModel ReturnType { get; set; }
    public string Body { get; set; }
    public bool Interface { get; set; }
}
