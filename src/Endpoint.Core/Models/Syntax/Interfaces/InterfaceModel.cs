using System.Collections.Generic;
using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Properties;
using Endpoint.Core.Models.Syntax.Types;

namespace Endpoint.Core.Models.Syntax.Interfaces;

public class InterfaceModel : TypeDeclarationModel
{
    public InterfaceModel(string name)
        :base(name)
    {
        Properties = new List<PropertyModel>();
        Implements = new List<TypeModel>();
        Methods = new List<MethodModel>();
        UsingDirectives = new List<UsingDirectiveModel>();
    }
    public List<PropertyModel> Properties { get; set; }
    public List<TypeModel> Implements { get; set; }
    public List<MethodModel> Methods { get; set; }
    public List<UsingDirectiveModel> UsingDirectives { get; set; }
}