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
        private string _returnType;
        private List<string> _parameters;
        private bool _static;
        private bool _override;

        public MethodSignatureBuilder()
        {
            _string = new();
            _async = true;
            _accessModifier = "public";
            _returnType = "void";
            _parameters = new();
            _indent = 0;
            _override = false;
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

        public MethodSignatureBuilder WithOverride(bool @override = true)
        {
            _override = @override;
            return this;
        }

        public MethodSignatureBuilder WithReturnType(string returnType)
        {
            _returnType = returnType;
            return this;
        }

        public MethodSignatureBuilder IsStatic(bool @static)
        {
            _static = @static;
            return this;
        }

        public MethodSignatureBuilder WithAsync(bool async)
        {
            _async = async;
            return this;
        }

        public MethodSignatureBuilder WithName(string name)
        {
            _methodName = name;
            return this;
        }

        public MethodSignatureBuilder WithAccessModifier(AccessModifier accessModifier)
        {
            _accessModifier = accessModifier switch { 
                AccessModifier.Public => "public",
                AccessModifier.Private => "private",
                AccessModifier.Protected => "protected",
                AccessModifier.Inherited => "",
                _ => throw new System.NotImplementedException()
            };
            return this;
        }

        public MethodSignatureBuilder WithEndpointType(EndpointType endpointType)
        {
            _methodName = endpointType switch
            {
                EndpointType.GetById => "GetById",
                EndpointType.Get => "Get",
                EndpointType.Create => "Create",
                EndpointType.Page => "Page",
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

            if (_override == true)
            {
                _string.Append("override");
                _string.Append(' ');
            }

            if (_static)
            {
                _string.Append("static");
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
