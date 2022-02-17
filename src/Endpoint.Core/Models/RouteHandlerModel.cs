using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class RouteHandlerModel
    {
        public string Pattern { get; set; }
        public string Name { get; set; }
        public string[] Body { get; set; }
        public List<ResponseModel> Responses { get; set; } = new List<ResponseModel>();

    }
}
