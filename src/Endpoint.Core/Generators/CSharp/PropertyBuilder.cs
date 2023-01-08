using Endpoint.Core.Enums;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using System.Text;

namespace Endpoint.Core.Builders
{
    public class PropertyBuilder
    {
        private StringBuilder _string;
        private int _indent;
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

        public PropertyBuilder WithAccessModifier(AccessModifier accessModifier)
        {
            _accessModifier = accessModifier switch
            {
                AccessModifier.Inherited => "",
                AccessModifier.Public => "public",
                AccessModifier.Private => "private",
                _ => throw new System.NotSupportedException()
            };

            return this;
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

        public string Build(SettingsModel settings = null, string resourceName = null)
        {
            if (!string.IsNullOrEmpty(_accessModifier))
            {
                _string.Append(_accessModifier)
                .Append(' ');
            }

            var content = _string.Append(_type)
                .Append(' ')
                .Append(_name)
                .Append(' ')
                .Append(_acessors)
                .ToString();

            if(settings != null && !string.IsNullOrEmpty(resourceName) && settings.IdDotNetType == IdPropertyType.Guid)
            {
                return $"{content}  = new {resourceName}Id(Guid.NewGuid());";
            }

            return content;
        }
    }
}
