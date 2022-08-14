namespace Endpoint.Core.Models.Sql
{
    public class ColumnModel
    {
        public string Name { get; set; }
        public SqlDataTypeModel Type { get; set; }
        public bool Nullable { get; set; }
    }
}
