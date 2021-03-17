using Endpoint.Application.Enums;
using Endpoint.Application.ValueObjects;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Application.Builders
{
    public class MethodBuilder
    {
        private List<string> _contents;
        private int _indent;
        private string _accessModifier;
        private List<string> _attributes;
        public List<string> _body;
        private bool _authorize;
        private EndpointType _endpointType;
        private string _resource;
        private bool _static;
        private string _name;
        private string _returnType;
        private List<string> _parameters;
        public MethodBuilder()
        {
            _accessModifier = "public";
            _contents = new List<string>();
            _attributes = new List<string>();
            _body = new List<string>();
            _authorize = false;
            _static = false;
            _name = "";
            _returnType = "";
            _parameters = new();
        }

        public MethodBuilder WithParameter(string paremeter)
        {
            _parameters.Add(paremeter);

            return this;
        }
        public MethodBuilder WithReturnType(string returnType)
        {
            _returnType = returnType;
            return this;
        }

        public MethodBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public MethodBuilder WithAuthorize(bool authorize)
        {
            this._authorize = authorize;
            return this;
        }

        public MethodBuilder WithPropertyName(string propertName)
        {
            return this;
        }

        public MethodBuilder IsStatic(bool isStatic = true)
        {
            this._static = isStatic;
            return this;
        }

        public MethodBuilder WithBody(List<string> body)
        {
            this._body = body;
            return this;
        }

        public MethodBuilder WithEndpointType(EndpointType endpointType)
        {
            _endpointType = endpointType;
            return this;
        }

        public MethodBuilder WithResource(string resource)
        {
            _resource = resource;
            return this;
        }

        public MethodBuilder WithIndent(int indent)
        {
            _indent = indent;
            return this;
        }

        public string[] Build()
        {
            if (_endpointType != default)
            {
                var requestType = _endpointType switch
                {
                    EndpointType.Create => $"Create{((Token)_resource).PascalCase}",
                    EndpointType.Delete => $"Delete{((Token)_resource).PascalCase}",
                    EndpointType.Get => $"Get{((Token)_resource).PascalCasePlural}",
                    EndpointType.GetById => $"Get{((Token)_resource).PascalCase}ById",
                    EndpointType.Update => $"Update{((Token)_resource).PascalCase}",
                    _ => throw new System.NotImplementedException()
                };

                _contents = AttributeBuilder.EndpointAttributes(_endpointType, _resource, _authorize).ToList();

                _contents.Add(new MethodSignatureBuilder()
                    .WithEndpointType(_endpointType)
                    .WithParameter(new ParameterBuilder($"{requestType}.Request", "request").WithFrom(From.Route).Build())
                    .WithAsync(true)
                    .WithReturnType(TypeBuilder.WithActionResult($"{requestType}.Response"))
                    .Build());

                _contents = _contents.Concat(new MethodBodyBuilder(_endpointType, 0, _resource).Build()).ToList();

                return _contents.ToArray();
            }

            var methodSignatureBuilder = new MethodSignatureBuilder()
                .WithName(_name)
                .IsStatic(_static)
                .WithAsync(false);
                
            foreach(var parameter in _parameters)
            {
                methodSignatureBuilder.WithParameter(parameter);
            }

            methodSignatureBuilder.WithReturnType(_returnType);

            _contents.Add(methodSignatureBuilder.Build());


            if(_body.Count > 0)
            {
                _contents.Add("{");
                foreach(var line in _body)
                {
                    _contents.Add(line);
                }
                _contents.Add("}");
            }
            
            return _contents.ToArray();
        }
    }
}
