using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax;

public class EntityModel
{
    public string AggregateRootName { get; private set; }
    public List<ClassProperty> Properties { get; protected set; } = new List<ClassProperty>();
    public string Name { get; set; }

    public EntityModel(string aggregateRootName, string name, List<ClassProperty> classProperties)
    {
        AggregateRootName = aggregateRootName;
        Name = name;
        Properties = classProperties;
    }

    public EntityModel(string name, List<ClassProperty> classProperties)
    {
        Name = name;
        Properties = classProperties;
    }

    public EntityModel(string name)
    {
        Name = name;
    }

    public EntityModel()
    {

    }

    public List<string> Usings { get; set; } = new List<string>();
    public string Namespace { get; set; }
}
