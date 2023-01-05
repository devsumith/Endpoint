using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax;

public class TypeModel
{
    public string Name { get; set; }
    public List<TypeModel> GenericTypeParameters { get; set; }
    public bool Nullable { get; set; }
    public bool Interface { get; set; }

}
