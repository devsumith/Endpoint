using Endpoint.Application.Enums;
using System.Collections.Generic;
using System.Text;

namespace Endpoint.Application.Builders
{
    public class MethodSignatureBuilder
    {
        private StringBuilder _string;
        private bool _async;
        private int _indent;
        private string _accessModifier;
        private string _methodName;
        private int _tab;
        private string _returnType;
        private List<string> _parameters;

        public MethodSignatureBuilder()
        {
            _string = new();
            _async = true;
            _accessModifier = "public";
            _tab = 4;
            _returnType = "void";
            _parameters = new();
            _indent = 0;
        }

        public MethodSignatureBuilder WithIndent(int indent)
        {
            _indent = indent;
            return this;
        }

        public MethodSignatureBuilder WithParameter(string parameter)
        {
            _parameters.Add(parameter);

            return this;
        }

        public MethodSignatureBuilder WithReturnType(string returnType)
        {
            _returnType = returnType;
            return this;
        }

        public MethodSignatureBuilder WithAsync(bool async)
        {
            _async = async;
            return this;
        }


        public MethodSignatureBuilder WithEndpointType(EndpointType endpointType)
        {
            _methodName = endpointType switch
            {
                EndpointType.GetById => "GetById",
                EndpointType.Get => "Get",
                EndpointType.Create => "Create",
                _ => throw new System.NotImplementedException()
            };

            return this;
        }

        public string Build()
        {
            EnsureValid();

            if (!string.IsNullOrEmpty(_accessModifier)) {
                _string.Append(_accessModifier);
                _string.Append(' ');
            }

            if (_async)
            {
                _string.Append("async");
                _string.Append(' ');
            }

            _string.Append(_returnType);
            
            _string.Append(' ');

            _string.Append(_methodName);

            _string.Append('(');

            _string.Append(string.Join(", ", _parameters));

            _string.Append(')');

            return _string.ToString();
        }

        public void EnsureValid()
        {
            if (_async && !_returnType.Contains("Task"))
            {
                throw new System.Exception($"Method signature is async but is returing {_returnType}");
            }
        }
    }
}
