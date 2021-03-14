using Endpoint.Application.Enums;
using System.Collections.Generic;

namespace Endpoint.Application.Builders
{
    public class MethodBuilder
    {
        private List<string> _contents;
        private int _indent;
        private int _tab = 4;
        private string _accessModifier;
        private List<string> _attributes;
        public List<string> _body;
        private bool _authorize;
        private EndpointType _endpointType;

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

        public string[] Build()
        {
            return _contents.ToArray();
        }
    }
}
