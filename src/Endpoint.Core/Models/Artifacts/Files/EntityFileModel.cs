using Endpoint.Core.Models.Syntax.Entities;

namespace Endpoint.Core.Models.Artifacts.Files
{
    public class EntityFileModel : FileModel
    {
        public EntityFileModel()
        {

        }
        public EntityModel Entity { get; set; }
    }
}
