using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class RouteHandlerModel
    {
        public string Pattern { get; set; }
        public string Name { get; set; }
        public string[] Body { get; set; }
        public RouteHandlerType Type { get; set; }
        public List<InputParameterModel> InputParameters { get; set; } = new List<InputParameterModel>();
        public List<ResponseModel> Responses { get; set; } = new List<ResponseModel>();

    }
}
