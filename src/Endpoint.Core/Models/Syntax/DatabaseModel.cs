using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax
{
    public class DatabaseModel
    {
        public string Name { get; set; }
        public List<SchemaModel> Schemas { get; set; }
    }
}
