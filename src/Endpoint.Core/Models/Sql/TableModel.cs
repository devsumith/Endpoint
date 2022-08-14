using System.Collections.Generic;

namespace Endpoint.Core.Models.Sql
{
    public class TableModel
    {
        public string Name { get; set; }
        public List<ColumnModel> Columns { get; set; }
        public List<ConstraintModel> Constraints { get; set; }
        public List<IndexModel> Indexes { get; set; }
    }
}
