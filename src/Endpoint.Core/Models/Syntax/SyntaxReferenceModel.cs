namespace Endpoint.Core.Models.Syntax;

public class SyntaxReferenceModel {

    public SyntaxReferenceModel(string syntax)
    {
        Syntax = syntax;
    }

    public string Syntax { get; set; }
}
