using System.Text;

namespace Endpoint.Application.Builders
{
    public class AccessorsBuilder
    {
        private StringBuilder _string;
        private string _accessor;
        private string _setAccessModifier;
        private bool _getterOnly;

        public AccessorsBuilder()
        {
            _string = new StringBuilder();
            _setAccessModifier = "";
        }

        public AccessorsBuilder WithGetterOnly(bool getterOnly)
        {
            _getterOnly = getterOnly;
            return this;
        }

        public AccessorsBuilder WithSetAccessModifuer(string accessModifier)
        {
            _setAccessModifier = accessModifier;
            return this;
        }

        public string Build()
        {
            if(_getterOnly)
                return _string.Append("{ ")
                .Append($"get;")
                .Append(" }").ToString();
            
            if(string.IsNullOrEmpty(_setAccessModifier))
                return _string.Append("{ ")
                .Append($"get; set;")
                .Append(" }").ToString();
            
            return _string.Append("{ ")
                .Append($"get; {_setAccessModifier} set;")
                .Append(" }").ToString();
        }
    }
}
