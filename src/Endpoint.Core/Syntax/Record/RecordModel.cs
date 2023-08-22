using System;

namespace Endpoint.Core.Syntax.Record;

public class RecordModel {
    public RecordModel(string name)
    {
        Name = name;
    }

    public string Name { get; set; }
    public RecordType Type { get; set; } = RecordType.Struct;
}


public enum RecordType
{
    Class,
    Struct
}