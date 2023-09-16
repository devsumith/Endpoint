namespace Endpoint.Core.Syntax.Records;

public class RecordModel : SyntaxModel
{
    public RecordModel(string name)
    {
        Name = name;
    }

    public string Name { get; set; }

    public RecordType Type { get; set; } = RecordType.Struct;
}
