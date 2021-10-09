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
        private Dictionary<string, string> _baseParameters;
        private string _name;
        private int _indent;
        private AccessModifier _accessModifier = AccessModifier.Inherited;
        private List<string> _ruleFors;
        private List<string> _body = new List<string>();
        private void Indent() { _indent++; }
        private void Unindent() { _indent--; }

        public ConstructorBuilder(string name)
        {
            _contents = new List<string>();
            _parameters = new Dictionary<string, string>();
            _baseParameters = new Dictionary<string, string>();
            _indent = 0;
            _name = name;
            _ruleFors = new();
        }

        public ConstructorBuilder WithRuleFors(List<string> ruleFors)
        {
            this._ruleFors = ruleFors;
            return this;
        }

        public ConstructorBuilder WithBody(List<string> body)
        {
            this._body = body;

            return this;
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

        public ConstructorBuilder WithBaseDependency(string type, string name)
        {
            _baseParameters.Add(type, name);
            return this;
        }

        public ConstructorBuilder WithParameters(Dictionary<string,string> parameters)
        {
            this._parameters = parameters;
            return this;
        }

        public ConstructorBuilder WithBaseParameters(Dictionary<string, string> parameters)
        {
            this._baseParameters = parameters;
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

            var allParameters = _parameters.Concat(_baseParameters);

            for(var i = 0; i < allParameters.Count(); i++)
            {
                signature.Append($"{allParameters.ElementAt(i).Key} {allParameters.ElementAt(i).Value}");

                if(i < allParameters.Count() -1)
                {
                    signature.Append(", ");
                }
            }

            signature.Append(')');

            return signature.ToString().Indent(_indent);
        }

        public string[] BuildBody()
        {
            var body = new List<string>();

            if (_parameters.Count() == 1 && _baseParameters.Count == 0 && _ruleFors.Count == 0 && _body.Count == 0)
            {
                Indent();
                body.Add($"=> _{_parameters.ElementAt(0).Value} = {_parameters.ElementAt(0).Value};".Indent(_indent));
                return body.ToArray();
            }

            if (_parameters.Count() == 0 && _baseParameters.Count > 0 && _ruleFors.Count == 0 && _body.Count == 0)
            {
                var line = new StringBuilder();

                Indent();

                line.Append($":base({string.Join(", ",_baseParameters.Select(x => x.Value))})".Indent(_indent));
                
                line.Append(" { }");
                
                body.Add(line.ToString());

                return body.ToArray();
            }

            if (_parameters.Count() == 0 && _baseParameters.Count > 0 && _ruleFors.Count == 0 && _body.Count > 0)
            {
                var line = new StringBuilder();

                Indent();

                line.Append($":base({string.Join(", ", _baseParameters.Select(x => x.Value))})".Indent(_indent));

                body.Add("{".Indent(_indent));

                Indent();

                foreach (var content in _body)
                {
                    body.Add(content.Indent(_indent));
                }

                Unindent();

                body.Add("}".Indent(_indent));

                return body.ToArray();
            }

            if (_parameters.Count == 0 && _baseParameters.Count == 0 && _ruleFors.Count > 0 && _body.Count == 0)
            {

                body.Add("{".Indent(_indent));

                Indent();

                foreach(var r in _ruleFors)
                {
                    body.Add(r.Indent(_indent));
                }

                Unindent();

                body.Add("}".Indent(_indent));
                
                return body.ToArray();
            }

            if (_parameters.Count == 0 && _baseParameters.Count == 0 && _ruleFors.Count == 0 && _body.Count > 0)
            {

                body.Add("{".Indent(_indent));

                Indent();

                foreach (var content in _body)
                {
                    body.Add(content.Indent(_indent));
                }

                Unindent();

                body.Add("}".Indent(_indent));

                return body.ToArray();
            }

            throw new NotSupportedException();
        }

        public string[] Build()
        {            
            _contents.Add(BuildSignature());

            foreach(var line in BuildBody())
            {
                _contents.Add(line);
            }

            return _contents.ToArray();
        }
    }
}
