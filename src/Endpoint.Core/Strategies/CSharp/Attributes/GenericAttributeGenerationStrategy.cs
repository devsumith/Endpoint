// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Builders.Statements;
using Endpoint.Core.Enums;
using Endpoint.Core.Models.Options;
using Endpoint.Core.Models.Syntax;
using Endpoint.Core.Models.Syntax.Attributes;
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

        public static string[] EndpointAttributes(SettingsModel settings, RouteType routeType, string resource, bool authorize = false, int indent = 0)
        {
            var attributes = new List<string>();

            var requestType = routeType switch
            {
                RouteType.Create => $"Create{((SyntaxToken)resource).PascalCase}",
                RouteType.Delete => $"Remove{((SyntaxToken)resource).PascalCase}",
                RouteType.Get => $"Get{((SyntaxToken)resource).PascalCasePlural}",
                RouteType.GetById => $"Get{((SyntaxToken)resource).PascalCase}ById",
                RouteType.Update => $"Update{((SyntaxToken)resource).PascalCase}",
                RouteType.Page => $"Get{((SyntaxToken)resource).PascalCasePlural}Page",
                _ => throw new System.NotImplementedException()
            };

            if (authorize)
            {
                attributes.Add(new GenericAttributeGenerationStrategy().WithName("Authorize").WithIndent(indent).Build());
            }

            if (routeType == RouteType.GetById)
            {
                attributes.AddRange(new SwaggerAnnotationBuilder($"Get {((SyntaxToken)resource).PascalCase} by id.", $"Get {((SyntaxToken)resource).PascalCase} by id.", indent).Build());
                attributes.Add(WithHttp(HttpVerbs.Get, HttpAttributeIdTemplateBuilder.Build(settings,resource), $"{((SyntaxToken)requestType).CamelCase}", indent));
                attributes.Add(WithProducesResponseType(HttpStatusCode.NotFound, "string", indent: indent));
            }

            if (routeType == RouteType.Delete)
            {
                attributes.AddRange(new SwaggerAnnotationBuilder($"Delete {((SyntaxToken)resource).PascalCase}.", $"Delete {((SyntaxToken)resource).PascalCase}.", indent).Build());
                attributes.Add(WithHttp(HttpVerbs.Delete, HttpAttributeIdTemplateBuilder.Build(settings, resource), $"{((SyntaxToken)requestType).CamelCase}", indent));
            }

            if (routeType == RouteType.Get)
            {
                attributes.AddRange(new SwaggerAnnotationBuilder($"Get {((SyntaxToken)resource).PascalCasePlural}.", $"Get {((SyntaxToken)resource).PascalCasePlural}.", indent).Build());
                attributes.Add(WithHttp(HttpVerbs.Get, routeName: $"{((SyntaxToken)requestType).CamelCase}", indent: indent));
            }

            if (routeType == RouteType.Create)
            {
                attributes.AddRange(new SwaggerAnnotationBuilder($"Create {((SyntaxToken)resource).PascalCase}.", $"Create {((SyntaxToken)resource).PascalCase}.", indent).Build());
                attributes.Add(WithHttp(HttpVerbs.Post, routeName: $"{((SyntaxToken)requestType).CamelCase}", indent: indent));
            }

            if (routeType == RouteType.Update)
            {
                attributes.AddRange(new SwaggerAnnotationBuilder($"Update {((SyntaxToken)resource).PascalCase}.", $"Update {((SyntaxToken)resource).PascalCase}.", indent).Build());
                attributes.Add(WithHttp(HttpVerbs.Put, routeName: $"{((SyntaxToken)requestType).CamelCase}", indent: indent));
            }

            if (routeType == RouteType.Page)
            {
                attributes.AddRange(new SwaggerAnnotationBuilder($"Get {((SyntaxToken)resource).PascalCase} Page.", $"Get {((SyntaxToken)resource).PascalCase} Page.", indent).Build());
                attributes.Add(WithHttp(HttpVerbs.Get, "page/{pageSize}/{index}", $"{((SyntaxToken)requestType).CamelCase}", indent));
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

