namespace Endpoint.Core.Models.Syntax
{
    public class ColumnModel
    {
        public string Name { get; set; }
        public SqlDataTypeModel Type { get; set; }
        public bool Nullable { get; set; }
    }
}
