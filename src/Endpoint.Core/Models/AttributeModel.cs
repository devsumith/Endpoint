using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class AttributeModel
    {
        public AttributeType Type { get; private set; }
        public string Name { get; private set; }
        public Dictionary<string,string> Properties { get; private set; }
        public List<string> Params { get; set; }
    }
}
