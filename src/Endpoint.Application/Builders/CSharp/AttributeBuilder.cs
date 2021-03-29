using Endpoint.Application.Enums;
using Endpoint.Application.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Endpoint.Application.Builders
{
    public class AttributeBuilder
    {
        private StringBuilder _string;
        private int _indent;
        private int _tab = 4;
        private string _name;
        private Dictionary<string, string> _properties;
        private List<string> _params;
        public AttributeBuilder()
        {
            _string = new();

            _properties = new();
            _params = new();
        }
        public AttributeBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public AttributeBuilder WithIndent(int indent)
        {
            _indent = indent;
            return this;
        }

        public AttributeBuilder WithParam(string param)
        {
            _params.Add(param);
            return this;
        }

        public AttributeBuilder WithProperty(string name, string value)
        {
            _properties.Add(name, value);

            return this;
        }

        public static string WithHttp(HttpVerbs verb, string route = null, string routeName = null, int indent = 0)
        {
            var builder = new AttributeBuilder()
                .WithIndent(indent)
                .WithName(verb switch
                {
                    HttpVerbs.Get => "HttpGet",
                    HttpVerbs.Post => "HttpPost",
                    _ => throw new System.NotImplementedException()
                });

            if(!string.IsNullOrEmpty(route))
            {
                builder.WithParam($"\"{route}\"");
            }

            if(!string.IsNullOrEmpty(routeName))
            {
                builder.WithProperty("Name", routeName);
            }

            return builder.Build();
        }

        public static string WithProducesResponseType(HttpStatusCode statusCode, string type = null, int indent = 0)
        {
            var builder = new AttributeBuilder()
                .WithName("ProducesResponseType")
                .WithIndent(indent);

            if(!string.IsNullOrEmpty(type))
            {
                builder.WithParam($"typeof({type})");
            }

            return builder.WithParam(statusCode switch
            {
                HttpStatusCode.InternalServerError => "(int)HttpStatusCode.InternalServerError",
                HttpStatusCode.BadRequest => "(int)HttpStatusCode.BadRequest",
                HttpStatusCode.OK => "(int)HttpStatusCode.OK",
                HttpStatusCode.NotFound => "(int)HttpStatusCode.NotFound",
                _ => throw new System.NotImplementedException()
            }).Build();
        }

        public static string[] EndpointAttributes(EndpointType endpointType, string resource, bool authorize = false, int indent = 0)
        {
            var attributes = new List<string>();

            var requestType = endpointType switch
            {
                EndpointType.Create => $"Create{((Token)resource).PascalCase}",
                EndpointType.Delete => $"Delete{((Token)resource).PascalCase}",
                EndpointType.Get => $"Get{((Token)resource).PascalCasePlural}",
                EndpointType.GetById => $"Get{((Token)resource).PascalCase}ById",
                EndpointType.Update => $"Update{((Token)resource).PascalCase}",
                _ => throw new System.NotImplementedException()
            };

            if (authorize)
            {
                attributes.Add(new AttributeBuilder().WithName("Authorize").WithIndent(indent).Build());
            }
            
            if(endpointType == EndpointType.GetById)
            {
                attributes.Add(WithHttp(HttpVerbs.Get, "{" + ((Token)resource).CamelCase + "Id}", $"{requestType}Route", indent));
                attributes.Add(WithProducesResponseType(HttpStatusCode.NotFound, "string", indent: indent));
            }

            if (endpointType == EndpointType.Get)
            {
                attributes.Add(WithHttp(HttpVerbs.Get, routeName: $"{requestType}Route", indent: indent));
            }

            if (endpointType == EndpointType.Create)
            {
                attributes.Add(WithHttp(HttpVerbs.Post, routeName: $"{requestType}Route", indent: indent));
            }

            attributes.Add(WithProducesResponseType(HttpStatusCode.InternalServerError, indent: indent));
            attributes.Add(WithProducesResponseType(HttpStatusCode.BadRequest, "ProblemDetails", indent: indent));
            attributes.Add(WithProducesResponseType(HttpStatusCode.OK, $"{requestType}.Response", indent: indent));

            return attributes.ToArray();           
        }


        public string Build()
        {
            for (var i = 0; i < _indent * _tab; i++) { _ = this._string.Append(' '); }

            _string.Append('[');

            _string.Append(_name);

            if (this._params.Count > 0)
            {
                _string.Append('(');

                _string.Append(string.Join(", ", _params));

                if (this._properties.Count > 0)
                {
                    _string.Append(", ");

                    _string.Append(string.Join(", ", _properties.Select(x => $"{x.Key} = \"{x.Value}\"")));
                }

                _string.Append(')');
            } 
            else
            {
                if (_properties.Count > 0)
                {
                    _string.Append('(');

                    _string.Append(string.Join(", ", _properties.Select(x => $"{x.Key} = \"{x.Value}\"")));

                    _string.Append(')');
                }
            }

            _string.Append(']');
            
            return _string.ToString();
        }
    }
}
