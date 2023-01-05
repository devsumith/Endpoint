using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax;

public class InterfaceModel: TypeDeclarationModel
{
    public InterfaceModel(string name)
    {
        Name = name;
        Properties = new List<PropertyModel>();
        Implements = new List<TypeModel>();
        Methods = new List<MethodModel>();
        UsingDirectives = new List<UsingDirectiveModel>();
    }
    public string Name { get; set; }
    public IList<PropertyModel> Properties { get; set; }
    public List<TypeModel> Implements { get; set; }
    public List<MethodModel> Methods { get; set; }
    public IList<UsingDirectiveModel> UsingDirectives { get; set; }
}