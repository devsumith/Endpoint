using System.Collections.Generic;

namespace Endpoint.Application.Builders
{
    public class NamespaceBuilder
    {
        private List<string> _usings;
        private string _name;
        private List<string> _classes;

        public NamespaceBuilder(string name)
        {
            _name = name;
            _usings = new List<string>();
            _classes = new List<string>();
        }

        public NamespaceBuilder WithUsing(string @using)
        {
            _usings.Add(@using);
            return this;
        }

        public NamespaceBuilder WithClass(string @class)
        {
            _classes.Add(@class);
            return this;
        }

        public string[] Build()
        {
            var content = new List<string>();
            return content.ToArray();
        }
    }
}
