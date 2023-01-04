namespace Endpoint.Core.Models.Syntax
{
    public class ConstraintModel
    {
        public string Name { get; set; }
        public Key Key { get; set; } = Key.Primary;
        public string ColumnName { get; set; }
        public string ReferencesSchemaName { get; set; }
        public string ReferencesTableName { get; set; }
        public string ReferencesColumnName { get; set; }
    }
}
