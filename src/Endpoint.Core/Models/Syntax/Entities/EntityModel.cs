// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax.Classes;

namespace Endpoint.Core.Models.Syntax.Entities;

public class EntityModel : ClassModel
{
    public string AggregateRootName { get; private set; }

    public EntityModel(string name)
        : base(name)
    {
        Name = name;
    }

    public ClassModel CreateDto()
    {
        var classModel = new ClassModel($"{Name}Dto");



        return classModel;
    }
}

