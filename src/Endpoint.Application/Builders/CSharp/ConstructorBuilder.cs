using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Endpoint.Application.Enums;
using Endpoint.Application.Extensions;

namespace Endpoint.Application.Builders.CSharp
{
    public class ConstructorBuilder
    {
        private List<string> _contents;
        private Dictionary<string, string> _parameters;
        private string _name;
        private int _indent;
        private AccessModifier _accessModifier = AccessModifier.Inherited;

        private void Indent() { _indent++; }
        private void Unindent() { _indent--; }

        public ConstructorBuilder(string name)
        {
            _contents = new List<string>();
            _parameters = new Dictionary<string, string>();
            _indent = 0;
            _name = name;
        }

        public ConstructorBuilder WithAccessModifier(AccessModifier accessModifier)
        {
            this._accessModifier = accessModifier;
            return this;
        }

        public ConstructorBuilder WithIndent(int indent)
        {
            this._indent = indent;
            return this;
        }

        public ConstructorBuilder WithDependency(string type, string name)
        {
            _parameters.Add(type, name);
            return this;
        }

        public ConstructorBuilder WithParameters(Dictionary<string,string> parameters)
        {
            this._parameters = parameters;
            return this;
        }

        public string BuildSignature()
        {
            var signature = new StringBuilder();

            if(_accessModifier != AccessModifier.Inherited)
            {
                signature.Append(_accessModifier switch { 
                    AccessModifier.Public => "public",
                    _ => throw new NotImplementedException()
                });

                signature.Append(' ');
            }

            signature.Append($"{_name}(");

            for(var i = 0; i < _parameters.Count; i++)
            {
                signature.Append($"{_parameters.ElementAt(i).Key} {_parameters.ElementAt(i).Value}");

                if(i < _parameters.Count -1)
                {
                    signature.Append(", ");
                }
            }
            signature.Append(')');

            return signature.ToString();
        }

        public string[] BuildBody()
        {
            throw new System.NotImplementedException();
        }

        public string[] Build()
        {            
            _contents.Add(BuildSignature());

            return _contents.ToArray();
        }
    }
}
