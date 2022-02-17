using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class WebApplicationModel
    {
        public List<RouteHandlerModel> RouteHandlers { get; set; } = new List<RouteHandlerModel>();
        public string DbContextName { get; set; }
        public string ApiNamespace { get; set; }
    }
}
