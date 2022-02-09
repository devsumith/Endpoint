using System.Collections.Generic;

namespace Endpoint.SharedKernal.Models
{
    public class ClassProperty
    {
        public string AccessModifier { get; private set; }
        public string Type { get; private set; }
        public List<ClassPropertyAccessor> Accessors { get; private set; } = new();
        public string Name { get; private set; }
        public bool Required { get; private set; }
        public bool Key { get; private set; }

        public ClassProperty(string accessModifier, string type, string name, List<ClassPropertyAccessor> accessors, bool required = true, bool key = false)
        {
            AccessModifier = accessModifier;
            Type = type;
            Accessors = accessors;
            Name = name;
            Required = required;
            Key = key;
        }
    }
}
