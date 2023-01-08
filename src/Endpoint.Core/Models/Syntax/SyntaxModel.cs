namespace Endpoint.Core.Models.Syntax;

public class SyntaxModel
{
    public SyntaxModel(IdPropertyType idPropertyType, IdPropertyFormat idPropertyFormat)
    {
        IdPropertyType = idPropertyType;
        IdPropertyFormat = idPropertyFormat;
    }

    public IdPropertyType IdPropertyType { get; set; }
    public IdPropertyFormat IdPropertyFormat { get; init; }
}
