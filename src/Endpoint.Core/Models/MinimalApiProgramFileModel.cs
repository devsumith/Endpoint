using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class MinimalApiProgramFileModel: FileModel
    {
        public List<string> Usings { get; set; } = new();
        public bool SwaggerEnabled { get; set; }
        public List<AggregateRootModel> Aggregates { get; set; } = new();
        public List<EndpointModel> Endpoints { get; set; } = new();
        public string DbContextName { get; init; }
    }
}
