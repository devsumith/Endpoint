
using System.Net;


namespace Endpoint.Core.Syntax;


public class ProducesAttributeModel
{
    public string StatusCode => HttpStatusCode switch
    {
        HttpStatusCode.OK => "",
        _ => ""
    };

    public string Type { get; private set; }
    public HttpStatusCode HttpStatusCode { get; private set; }

    public ProducesAttributeModel(string type, HttpStatusCode httpStatusCode)
    {
        Type = type;
        HttpStatusCode = HttpStatusCode;
    }
}

