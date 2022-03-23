using Endpoint.Core.Enums;

namespace Endpoint.Core.Models
{
    public class EndpointModel
    {
        public bool AuthorizationRequired { get; set; }
        public EndpointType Type { get; set; }
        public AggregateRootModel Aggregate { get; set; }
    }
}
