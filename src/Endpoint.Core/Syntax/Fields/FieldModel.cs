// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Syntax.Types;

namespace Endpoint.Core.Syntax.Fields;

public class FieldModel : SyntaxModel
{
    public FieldModel()
    {
        AccessModifier = AccessModifier.Private;
        ReadOnly = true;
    }

    public TypeModel Type { get; set; }
    public string Name { get; set; }
    public bool ReadOnly { get; set; }
    public string DefaultValue { get; set; }
    public AccessModifier AccessModifier { get; set; }

    public static FieldModel LoggerOf(string name)
    {
        return new FieldModel()
        {
            Type = TypeModel.LoggerOf(name),
            Name = "_logger"
        };
    }

    public static FieldModel Mediator => new FieldModel()
    {
        Type = new TypeModel($"IMediator"),
        Name = "_mediator"
    };

}