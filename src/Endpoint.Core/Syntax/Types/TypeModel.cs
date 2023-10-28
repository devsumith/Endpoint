// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using System.Collections.Generic;
using Endpoint.Core.Syntax.Classes;

namespace Endpoint.Core.Syntax.Types;

public class TypeModel : SyntaxModel
{
    public static TypeModel Task = new TypeModel("Task");

    public TypeModel(string name = null)
    {
        Name = name;
        GenericTypeParameters = new List<TypeModel>();
    }

    public ClassModel Class { get; set; }

    public string Name { get; set; }

    public List<TypeModel> GenericTypeParameters { get; set; }

    public bool Nullable { get; set; }

    public bool Interface { get; set; }

    public static TypeModel TaskOf(string typeName)
    {
        return new TypeModel("Task")
        {
            GenericTypeParameters = new List<TypeModel>()
            {
                new TypeModel(typeName),
            },
        };
    }

    public static TypeModel DbSetOf(string entityName)
        => new ("DbSet")
        {
            GenericTypeParameters = new ()
                    {
                        new (entityName),
                    },
        };

    public static TypeModel LoggerOf(string typeName)
    {
        return new TypeModel("ILogger")
        {
            GenericTypeParameters = new List<TypeModel>()
            {
                new TypeModel(typeName),
            },
        };
    }

    public static TypeModel ListOf(string typeName)
    {
        return new TypeModel("List")
        {
            GenericTypeParameters = new List<TypeModel>()
            {
                new TypeModel(typeName),
            },
        };
    }

    public static TypeModel CreateTaskOfActionResultOf(string typeName)
    {
        return new TypeModel("Task")
        {
            GenericTypeParameters = new List<TypeModel>
                {
                    new TypeModel("ActionResult")
                    {
                        GenericTypeParameters = new List<TypeModel>()
                        {
                            new TypeModel(typeName),
                        },
                    },
                },
        };
    }
}
