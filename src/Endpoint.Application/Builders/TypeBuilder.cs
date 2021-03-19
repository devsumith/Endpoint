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

        public static string WithActionResult(string type)
            => new TypeBuilder().WithGenericType("Task", new TypeBuilder().WithGenericType("ActionResult", type).Build()).Build();

        public TypeBuilder WithGenericType(string generic, string type)
        {
            _generic = generic;
            _type = type;
            return this;
        }

        public TypeBuilder WithGenericType(string generic, string type1, string type2)
        {
            _generic = generic;
            _type = $"{type1}, {type2}";
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
