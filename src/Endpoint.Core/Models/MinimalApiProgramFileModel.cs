using Endpoint.Core.Models.Files;
using System.Collections.Generic;

namespace Endpoint.Core.Models
{
    public class MinimalApiProgramFileModel: FileModel
    {
        public List<string> Usings { get; set; } = new();
        public bool SwaggerEnabled { get; set; }
        public List<Entity> Entities { get; set; } = new();
        public List<RouteHandlerModel> RouteHandlers { get; set; } = new();
        public string DbContextName { get; init; }
    }
}
