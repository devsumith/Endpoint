// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Syntax.Attributes;
using Endpoint.Core.Syntax.Constructors;
using Endpoint.Core.Syntax.Fields;
using Endpoint.Core.Syntax.Interfaces;
using Endpoint.Core.Syntax.Methods;

namespace Endpoint.Core.Syntax.Classes;

public class ClassModel : InterfaceModel
{
    public ClassModel()
    {
    }

    public ClassModel(string name)
        : base(name)
    {
        Fields = new List<FieldModel>();
        Constructors = new List<ConstructorModel>();
        Attributes = new List<AttributeModel>();
        AccessModifier = AccessModifier.Public;
    }

    public AccessModifier AccessModifier { get; set; }

    public List<FieldModel> Fields { get; set; }

    public List<ConstructorModel> Constructors { get; set; }

    public List<AttributeModel> Attributes { get; set; }

    public bool Static { get; set; }

    public override void AddMethod(MethodModel method)
    {
        method.IsInterface = false;
        Methods.Add(method);
    }

    public ClassModel CreateDto()
        => new ClassModel($"{Name}Dto")
        {
            Properties = Properties,
        };

    public override IEnumerable<SyntaxModel> GetChildren()
    {
        foreach (var method in Methods)
        {
            yield return method;
        }

        foreach (var method in Fields)
        {
            yield return method;
        }

        foreach (var method in Constructors)
        {
            yield return method;
        }

        foreach (var method in Attributes)
        {
            yield return method;
        }
    }
}
