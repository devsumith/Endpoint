using System.Collections.Generic;
using System.Text;

namespace Endpoint.Application.Builders
{
    public class TypeBuilder
    {
        private StringBuilder _string;
        private bool _isGeneric => !string.IsNullOrEmpty(_generic);
        private string _type;
        private string _generic;

        public TypeBuilder()
        {
            _string = new StringBuilder();
        }

        public TypeBuilder WithGenericType(string generic, string type)
        {
            _generic = generic;
            _type = type;
            return this;
        }
        public string Build()
        {
            return _isGeneric 
                ? _string.Append(_generic).Append('<').Append(_type).Append('>').ToString()
                : _string.Append(_type).ToString();
            
        }
    }
}
