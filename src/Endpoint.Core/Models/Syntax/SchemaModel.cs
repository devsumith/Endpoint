using System.Collections.Generic;

namespace Endpoint.Core.Models.Syntax
{
    public class SchemaModel
    {
        public string Name { get; set; }
        public List<TableModel> Tables { get; set; }
    }
}
