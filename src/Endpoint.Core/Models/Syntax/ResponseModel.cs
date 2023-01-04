using Endpoint.Core.Enums;
using System.Collections.Generic;
using System.Net;

namespace Endpoint.Core.Models.Syntax
{
    public class ResponseModel
    {
        public int StatusCode { get; set; }
        public string ResponseType { get; set; }
        public string ContentType { get; set; }
        public List<string> Params { get; set; }
    }

    public class ProducesModel
    {
        public string StatusCode => HttpStatusCode switch
        {
            HttpStatusCode.OK => "",
            _ => ""
        };

        public string Type { get; private set; }
        public HttpStatusCode HttpStatusCode { get; private set; }

        public ProducesModel(string type, HttpStatusCode httpStatusCode)
        {
            Type = type;
            HttpStatusCode = HttpStatusCode;
        }
    }
}
