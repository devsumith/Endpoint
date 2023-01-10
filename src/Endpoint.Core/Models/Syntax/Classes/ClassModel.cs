using System.Collections.Generic;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Attributes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Fields;
using Endpoint.Core.Models.Syntax.Interfaces;
using Endpoint.Core.Models.Syntax.Methods;

namespace Endpoint.Core.Models.Syntax.Classes;

public class ClassModel : InterfaceModel
{
    public ClassModel(string name)
        : base(name)
    {
        Fields = new List<FieldModel>();
        Constructors = new List<ConstructorModel>();
        Attributes = new List<AttributeModel>();
        AccessModifier = AccessModifier.Public;
    }

    public AccessModifier AccessModifier { get; set; }
    public List<FieldModel> Fields { get; set; }
    public List<ConstructorModel> Constructors { get; set; }
    public List<AttributeModel> Attributes { get; set; }    
    public bool Static { get; set; }

    public override void AddMethod(MethodModel method)
    {
        method.Interface = false;
        Methods.Add(method);
    }

}
