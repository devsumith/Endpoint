using Endpoint.Core.Models.Syntax.Methods;
using Endpoint.Core.Models.Syntax.Types;
using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax.Interfaces;

public class InterfaceModel : TypeDeclarationModel
{
    public InterfaceModel(string name)
        :base(name)
    {
        Implements = new List<TypeModel>();
        Methods = new List<MethodModel>();
    }

    public List<TypeModel> Implements { get; set; }
    public List<MethodModel> Methods { get; set; }

    public virtual void AddMethod(MethodModel method)
    {
        method.Interface = true;
        Methods.Add(method);
    }
}