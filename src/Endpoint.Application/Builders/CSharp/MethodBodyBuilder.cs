using Endpoint.Application.Enums;
using Endpoint.Application.Extensions;
using Endpoint.Application.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Endpoint.Application.Builders
{
    public class MethodBodyBuilder
    {
        private StringBuilder _string;
        private int _indent;
        private int _tab = 4;
        private EndpointType? _endpointType;
        private string _resource;

        public MethodBodyBuilder(EndpointType? endpointType = null, int indent = 0, string resource = null)
        {
            _string = new StringBuilder();
            _indent = indent;
            _endpointType = endpointType;
            _resource = resource;
        }

        public string[] Build()
        {
            var logStatementBuilder = new LogStatementBuilder(_resource,_endpointType);

            var logStatement = logStatementBuilder.Build();
            var body = new List<string>()
            {
                "{"
            };

            if(logStatement.Length > 0)
            {
                body.AddRange(logStatement.Select(x => x.Indent(_indent + 1)));
                body.Add("");
            }

            switch(_endpointType)
            {
                case EndpointType.GetById:
                    body.AddRange(BuildGetByIdEndpointBody(_resource));
                    break;

                case EndpointType.Get:
                    body.AddRange(BuildGetEndpointBody(_resource));
                    break;

                default:
                    body.AddRange(BuildEndpointBody());
                    break;
            }
            
            body.Add("}");

            return body.ToArray();

        }

        public string[] BuildEndpointBody()
            => new string[1]
            {
                "return await _mediator.Send(request);".Indent(_indent + 1)
            };

        public string[] BuildGetEndpointBody(string resource)
            => new string[1]
            {
                $"return await _mediator.Send(new Get{((Token)resource).PascalCasePlural}.Request());".Indent(_indent + 1)
            };

        public string[] BuildGetByIdEndpointBody(string resource)
            => new string[8]
            {
                "var response = await _mediator.Send(request);".Indent(_indent + 1),
                "",
                $"if (response.{((Token)resource).PascalCase} == null)".Indent(_indent + 1),
                "{".Indent(_indent + 1),
                $"return new NotFoundObjectResult(request.{((Token)resource).PascalCase}Id);".Indent(_indent + 2),
                "}".Indent(_indent + 1),
                "",
                "return response;".Indent(_indent + 1),
            };

        public string[] BuildCreateEndpointBody(string resource)
            => new string[8]
            {
                "var response = await _mediator.Send(request);".Indent(_indent + 1),
                "",
                $"if (response.{((Token)resource).PascalCase} == null)".Indent(_indent + 1),
                "{".Indent(_indent + 1),
                $"return new NotFoundObjectResult(request.{((Token)resource).PascalCase}Id);".Indent(_indent + 2),
                "}".Indent(_indent + 1),
                "",
                "return response;".Indent(_indent + 1),
            };

        public string[] BuildDeleteEndpointBody(string resource)
            => new string[8]
            {
                "var response = await _mediator.Send(request);".Indent(_indent + 1),
                "",
                $"if (response.{((Token)resource).PascalCase} == null)".Indent(_indent + 1),
                "{".Indent(_indent + 1),
                $"return new NotFoundObjectResult(request.{((Token)resource).PascalCase}Id);".Indent(_indent + 2),
                "}".Indent(_indent + 1),
                "",
                "return response;".Indent(_indent + 1),
            };

        public string[] BuildUpdateEndpointBody(string resource)
            => new string[8]
            {
                "var response = await _mediator.Send(request);".Indent(_indent + 1),
                "",
                $"if (response.{((Token)resource).PascalCase} == null)".Indent(_indent + 1),
                "{".Indent(_indent + 1),
                $"return new NotFoundObjectResult(request.{((Token)resource).PascalCase}Id);".Indent(_indent + 2),
                "}".Indent(_indent + 1),
                "",
                "return response;".Indent(_indent + 1),
            };
    }
}
