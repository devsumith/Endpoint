using Endpoint.Core.Builders.Statements;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.ValueObjects;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Endpoint.Core.Builders
{
    public class GenericAttributeGenerationStrategy
    {
        private StringBuilder _string;
        private int _indent;
        private int _tab = 4;
        private string _name;
        private Dictionary<string, string> _properties;
        private List<string> _params;
        public GenericAttributeGenerationStrategy()
        {
            _string = new();
            _properties = new();
            _params = new();
        }
        public GenericAttributeGenerationStrategy WithName(string name)
        {
            _name = name;
            return this;
        }

        public GenericAttributeGenerationStrategy WithIndent(int indent)
        {
            _indent = indent;
            return this;
        }

        public GenericAttributeGenerationStrategy WithParam(string param)
        {
            _params.Add(param);
            return this;
        }

        public GenericAttributeGenerationStrategy WithProperty(string name, string value)
        {
            _properties.Add(name, value);

            return this;
        }

        public static string WithHttp(HttpVerbs verb, string route = null, string routeName = null, int indent = 0)
        {
            var builder = new GenericAttributeGenerationStrategy()
                .WithIndent(indent)
                .WithName(verb switch
                {
                    HttpVerbs.Get => "HttpGet",
                    HttpVerbs.Post => "HttpPost",
                    HttpVerbs.Put => "HttpPut",
                    HttpVerbs.Delete => "HttpDelete",
                    _ => throw new System.NotImplementedException()
                });

            if (!string.IsNullOrEmpty(route))
            {
                builder.WithParam($"\"{route}\"");
            }

            if (!string.IsNullOrEmpty(routeName))
            {
                builder.WithProperty("Name", routeName);
            }

            return builder.Build();
        }

        public static string WithProducesResponseType(HttpStatusCode statusCode, string type = null, int indent = 0)
        {
            var builder = new GenericAttributeGenerationStrategy()
                .WithName("ProducesResponseType")
                .WithIndent(indent);

            if (!string.IsNullOrEmpty(type))
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

        public static string[] EndpointAttributes(SettingsModel settings, EndpointType endpointType, string resource, bool authorize = false, int indent = 0)
        {
            var attributes = new List<string>();

            var requestType = endpointType switch
            {
                EndpointType.Create => $"Create{((Token)resource).PascalCase}",
                EndpointType.Delete => $"Remove{((Token)resource).PascalCase}",
                EndpointType.Get => $"Get{((Token)resource).PascalCasePlural}",
                EndpointType.GetById => $"Get{((Token)resource).PascalCase}ById",
                EndpointType.Update => $"Update{((Token)resource).PascalCase}",
                EndpointType.Page => $"Get{((Token)resource).PascalCasePlural}Page",
                _ => throw new System.NotImplementedException()
            };

            if (authorize)
            {
                attributes.Add(new GenericAttributeGenerationStrategy().WithName("Authorize").WithIndent(indent).Build());
            }

            if (endpointType == EndpointType.GetById)
            {
                attributes.AddRange(new SwaggerAnnotationBuilder($"Get {((Token)resource).PascalCase} by id.", $"Get {((Token)resource).PascalCase} by id.", indent).Build());
                attributes.Add(WithHttp(HttpVerbs.Get, HttpAttributeIdTemplateBuilder.Build(settings,resource), $"{((Token)requestType).CamelCase}", indent));
                attributes.Add(WithProducesResponseType(HttpStatusCode.NotFound, "string", indent: indent));
            }

            if (endpointType == EndpointType.Delete)
            {
                attributes.AddRange(new SwaggerAnnotationBuilder($"Delete {((Token)resource).PascalCase}.", $"Delete {((Token)resource).PascalCase}.", indent).Build());
                attributes.Add(WithHttp(HttpVerbs.Delete, HttpAttributeIdTemplateBuilder.Build(settings, resource), $"{((Token)requestType).CamelCase}", indent));
            }

            if (endpointType == EndpointType.Get)
            {
                attributes.AddRange(new SwaggerAnnotationBuilder($"Get {((Token)resource).PascalCasePlural}.", $"Get {((Token)resource).PascalCasePlural}.", indent).Build());
                attributes.Add(WithHttp(HttpVerbs.Get, routeName: $"{((Token)requestType).CamelCase}", indent: indent));
            }

            if (endpointType == EndpointType.Create)
            {
                attributes.AddRange(new SwaggerAnnotationBuilder($"Create {((Token)resource).PascalCase}.", $"Create {((Token)resource).PascalCase}.", indent).Build());
                attributes.Add(WithHttp(HttpVerbs.Post, routeName: $"{((Token)requestType).CamelCase}", indent: indent));
            }

            if (endpointType == EndpointType.Update)
            {
                attributes.AddRange(new SwaggerAnnotationBuilder($"Update {((Token)resource).PascalCase}.", $"Update {((Token)resource).PascalCase}.", indent).Build());
                attributes.Add(WithHttp(HttpVerbs.Put, routeName: $"{((Token)requestType).CamelCase}", indent: indent));
            }

            if (endpointType == EndpointType.Page)
            {
                attributes.AddRange(new SwaggerAnnotationBuilder($"Get {((Token)resource).PascalCase} Page.", $"Get {((Token)resource).PascalCase} Page.", indent).Build());
                attributes.Add(WithHttp(HttpVerbs.Get, "page/{pageSize}/{index}", $"{((Token)requestType).CamelCase}", indent));
            }

            attributes.Add(WithProducesResponseType(HttpStatusCode.InternalServerError, indent: indent));
            attributes.Add(WithProducesResponseType(HttpStatusCode.BadRequest, "ProblemDetails", indent: indent));
            attributes.Add(WithProducesResponseType(HttpStatusCode.OK, $"{requestType}Response", indent: indent));

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

        public string[] Generate(AttributeModel model)
        {
            return new string[0] { };
        }
    }
}
