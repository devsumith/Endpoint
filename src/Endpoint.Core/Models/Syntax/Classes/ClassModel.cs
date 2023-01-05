using System.Collections.Generic;
using Endpoint.Core.Models.Syntax.Attributes;
using Endpoint.Core.Models.Syntax.Constructors;
using Endpoint.Core.Models.Syntax.Fields;

namespace Endpoint.Core.Models.Syntax.Classes;

public class ClassModel : InterfaceModel
{
    public ClassModel(string name)
        : base(name)
    {
        Fields = new List<FieldModel>();
        Constructors = new List<ConstructorModel>();
        Attributes = new List<AttributeModel>();

    }

    public IList<FieldModel> Fields { get; set; }
    public IList<ConstructorModel> Constructors { get; set; }
    public List<AttributeModel> Attributes { get; set; }
    public bool ForInterface { get; set; }
    public bool IsStatic { get; set; }

}
