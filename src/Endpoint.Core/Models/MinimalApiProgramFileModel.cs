using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class MinimalApiProgramFileModel: FileModel
    {
        public List<string> Usings { get; set; }
        public bool SwaggerEnabled { get; set; }
        public List<AggregateRootModel> Aggregates { get; set; }
        public List<EndpointModel> Endpoints { get; set; }
        public string DbContextName { get; init; }
    }
}
