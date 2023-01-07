using System.Collections.Generic;
using Endpoint.Core.Models.Syntax.Entities;
using Endpoint.Core.Models.Syntax.RouteHandlers;

namespace Endpoint.Core.Models.Artifacts.Files
{
    public class MinimalApiProgramFileModel : FileModel
    {
        public List<string> Usings { get; set; } = new();
        public bool SwaggerEnabled { get; set; }
        public List<EntityModel> Entities { get; set; } = new();
        public List<RouteHandlerModel> RouteHandlers { get; set; } = new();
        public string DbContextName { get; init; }
    }
}
