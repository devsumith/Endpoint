// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Attributes;
using Endpoint.Core.Models.Syntax.Types;

namespace Endpoint.Core.Models.Syntax.Params;

public class ParamModel
{
    public string Name { get; set; }
    public TypeModel Type { get; set; }
    public AttributeModel Attribute { get; set; }
    public string DefaultValue { get; set; }
    public bool ExtensionMethodParam { get; set; }

    public static ParamModel CancellationToken = new()
    {
        Type = new TypeModel("CancellationToken"),
        Name = "cancellationToken"
    };

    public static ParamModel LoggerOf(string name) => new()
    {
        Type = TypeModel.LoggerOf(name),
        Name = "logger"
    };

    public static ParamModel Mediator => new()
    {
        Type = new TypeModel($"IMediator"),
        Name = "mediator"
    };
}

