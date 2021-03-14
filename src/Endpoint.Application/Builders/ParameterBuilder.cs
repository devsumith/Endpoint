using Endpoint.Application.Enums;
using System.Text;

namespace Endpoint.Application.Builders
{
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
}
