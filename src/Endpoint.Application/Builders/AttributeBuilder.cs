using Endpoint.Application.Enums;
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
                HttpStatusCode.OK => "(int)HttpStatusCode.OK)",
                HttpStatusCode.NotFound => "(int)HttpStatusCode.NotFound",
                _ => throw new System.NotImplementedException()
            }).Build();
        }

        public static List<string> WithProducesResponseTypes(string type = null)
        {
            return new List<string>()
            {
                WithProducesResponseType(HttpStatusCode.InternalServerError),
                WithProducesResponseType(HttpStatusCode.BadRequest,"ProblemDetails"),
                WithProducesResponseType(HttpStatusCode.OK,type),
                WithProducesResponseType(HttpStatusCode.NotFound,"string")
            };
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