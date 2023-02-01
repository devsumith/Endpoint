// Copyright (c) Quinntyne Brown. All Rights Reserved.
// Licensed under the MIT License. See License.txt in the project root for license information.

using Endpoint.Core.Models.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace Endpoint.Core.Models.Artifacts.Files;

public class ObjectFileModel<T> : FileModel
{
    public ObjectFileModel(T @object, List<UsingDirectiveModel> usings, string name, string directory, string extension)
        :base(name.Split('.').Last(),directory, extension)
    {
        Object = @object;
        Usings = usings;
    }

    public T Object { get; init; }
    public List<UsingDirectiveModel> Usings { get; set; } = new List<UsingDirectiveModel>();

    public string Namespace { get; set; }
}

