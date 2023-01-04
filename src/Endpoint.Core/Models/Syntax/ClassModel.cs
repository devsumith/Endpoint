using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax;

public class ClassModel
{
    public List<ClassProperty> Properties { get; set; } = new List<ClassProperty>();

    public List<ConstructorModel> Constructors { get; set; } = new List<ConstructorModel>();

    public List<ClassMethodModel> Methods { get; set; } = new List<ClassMethodModel>();
}
