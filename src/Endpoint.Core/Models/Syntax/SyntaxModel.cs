namespace Endpoint.Core.Models.Syntax;

public class SyntaxModel
{
    public SyntaxModel(IdPropertyType idPropertyType, IdPropertyFormat idPropertyFormat, string dbContextName)
    {
        IdPropertyType = idPropertyType;
        IdPropertyFormat = idPropertyFormat;
        DbContextName = dbContextName;
    }

    public IdPropertyType IdPropertyType { get; set; }
    public IdPropertyFormat IdPropertyFormat { get; init; }
    public string DbContextName { get; set; }
}
