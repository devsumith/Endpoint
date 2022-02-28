using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class AttributeModel
    {
        public AttributeType Type { get; private set; }
        public string Name { get; private set; }
        public Dictionary<string,string> Properties { get; private set; }
        public List<string> Params { get; set; }
        public string Template { get; set; }
        public int Order { get; private set; } = 0;

        public AttributeModel(AttributeType type, string name, Dictionary<string,string> properties)
        {
            Type = type;
            Name = name;
            Properties = properties;
        }
    }
}
