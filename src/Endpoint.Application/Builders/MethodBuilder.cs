using Endpoint.Application.Enums;
using System.Collections.Generic;
using System.Text;

namespace Endpoint.Application.Builders
{
    public class MethodBuilder
    {
        private List<string> _contents;
        private int _indent;
        private int _tab = 4;
        private string _accessModifier = "public";
        private List<string> _attributes;
        private List<ParemeterInfo> _parameters = new List<ParemeterInfo>();
        public List<string> _body = new List<string>();


        public MethodBuilder()
        {
            _contents = new List<string>();
            _attributes = new List<string>();
            _body = new List<string>();
        }

        public string[] Build()
        {
            return _contents.ToArray();
        }
    }
    
    public class ParameterBuilder
    {
        private StringBuilder _string;
        private string _from;
        private string _type;
        private string _value;
        public ParameterBuilder(string type, string value)
        {
            _string = new StringBuilder();
            _type = type;
            _value = value;
        }

        public ParameterBuilder WithFrom(From from)
        {
            _from = from switch {
                From.Route => new AttributeBuilder().WithName("FromRoute").Build(),
                _ => throw new System.NotImplementedException()
            };

            return this;
        }

        public string Build()
        {
            if(!string.IsNullOrEmpty(_from))
            {
                _string.Append(_from);                
            }

            _string.Append(_type);

            _string.Append(' ');

            _string.Append(_value);

            return _string.ToString();
        }
    }

    public class ParemeterInfo
    {

    }
}
