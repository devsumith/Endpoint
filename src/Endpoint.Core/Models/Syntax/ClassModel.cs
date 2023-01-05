using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax;

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
