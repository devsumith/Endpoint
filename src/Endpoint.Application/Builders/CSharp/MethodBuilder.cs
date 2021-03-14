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

        public MethodBuilder()
        {
            _accessModifier = "public";
            _contents = new List<string>();
            _attributes = new List<string>();
            _body = new List<string>();
            _authorize = false;
        }

        public MethodBuilder WithAuthorize(bool authorize)
        {
            this._authorize = authorize;
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
    }
}
