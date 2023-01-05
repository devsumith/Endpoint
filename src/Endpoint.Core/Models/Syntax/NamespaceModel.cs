using System.Collections.Generic;
using Endpoint.Core.Models.Syntax.Classes;

namespace Endpoint.Core.Models.Syntax;

public class NamespaceModel
{
    public List<string> Usings { get; set; }
    public string Name { get; set; }
    public List<ClassModel> Classes { get; set; }
}
