using System.Collections.Generic;
using Endpoint.Core.Models.Syntax.Properties;

namespace Endpoint.Core.Models.Syntax.Entities;

public class EntityModel
{
    public string AggregateRootName { get; private set; }
    public List<PropertyModel> Properties { get; protected set; } = new List<PropertyModel>();
    public string Name { get; set; }

    public EntityModel(string aggregateRootName, string name, List<PropertyModel> classProperties)
    {
        AggregateRootName = aggregateRootName;
        Name = name;
        Properties = classProperties;
    }

    public EntityModel(string name, List<PropertyModel> classProperties)
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
