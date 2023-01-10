using Endpoint.Core.Enums;
using Endpoint.Core.Models.Syntax.Types;

namespace Endpoint.Core.Models.Syntax.Fields;

public class FieldModel
{
    public FieldModel()
    {
        AccessModifier = AccessModifier.Private;
        ReadOnly = true;
    }

    public TypeModel Type { get; set; }
    public string Name { get; set; }
    public bool ReadOnly { get; set; }
    public AccessModifier AccessModifier { get; set; }
}
