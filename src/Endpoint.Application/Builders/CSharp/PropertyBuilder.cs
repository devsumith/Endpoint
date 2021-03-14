using System.Text;

namespace Endpoint.Application.Builders
{
    public class PropertyBuilder
    {
        private StringBuilder _string;
        private int _indent;
        private int _tab = 4;
        private string _accessModifier;
        private string _type;
        private string _acessors;
        private string _name;
        public PropertyBuilder()
        {
            _string = new StringBuilder();
            _accessModifier = "public";
            _indent = 0;
            _type = "";
        }

        public PropertyBuilder WithIndent(int indent)
        {
            _indent = indent;
            return this;
        }

        public PropertyBuilder WithType(string type)
        {
            _type = type;
            return this;
        }

        public PropertyBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public PropertyBuilder WithAccessors(string accessors)
        {
            _acessors = accessors;
            return this;
        }

        public string Build()
        {
            for(var i = 0; i < _indent * _tab; i++) { _ = _string.Append(' '); }

            return _string.Append(_accessModifier)
                .Append(' ')
                .Append(_type)
                .Append(' ')
                .Append(_name)
                .Append(' ')
                .Append(_acessors)
                .ToString();
        }
    }
}
